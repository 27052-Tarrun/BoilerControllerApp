using BoilerController.Constants;
using BoilerController.Enums;
using BoilerController.Events;
using BoilerController.Exceptions;
using BoilerController.Models;

namespace BoilerController.Services;

/// <summary>
/// Provides boiler operations.
/// </summary>
public class BoilerService : IBoilerService
{
    private readonly IEventLogService _eventLogService;

    private readonly BoilerModel _boiler = new();
    private readonly object _boilerLock = new();
    private Task? _sequenceTask;
    private CancellationTokenSource? _sequenceCts;

    public BoilerModel Boiler
    {
        get
        {
            lock (_boilerLock)
            {
                return CopyBoiler(_boiler);
            }
        }
    }

    public event EventHandler<BoilerProgressEventArgs>? ProgressChanged;
    public event EventHandler? SequenceCompleted;

    public BoilerService(IEventLogService eventLogService)
    {
        _eventLogService = eventLogService;
        _boiler.Status = BoilerStatus.Lockout;
        _boiler.InterlockState = InterlockState.Open;
        _eventLogService.Log("Boiler Initialized", "System Started");
    }

    public void ToggleInterlock()
    {
        lock (_boilerLock)
        {
            _boiler.InterlockState = _boiler.InterlockState == InterlockState.Open ? InterlockState.Closed : InterlockState.Open;
            if (_boiler.InterlockState == InterlockState.Open)
            {
                _sequenceCts?.Cancel();
                _boiler.Status = BoilerStatus.Lockout;
                _boiler.CurrentCycle = null;

                _eventLogService.Log("Boiler Status Changed", "Lockout");
            }
        }

        _eventLogService.Log("Interlock Switch", $"Toggled To {_boiler.InterlockState}");
    }

    public void ResetLockout()
    {
        lock (_boilerLock)
        {
            if (_boiler.Status == BoilerStatus.Running)
            {
                _sequenceCts?.Cancel();

                _boiler.Status = BoilerStatus.Lockout;
                _boiler.CurrentCycle = null;
                _eventLogService.Log("Boiler Status Changed", "Lockout");
                return;
            }

            if (_boiler.InterlockState == InterlockState.Open)
            {
                throw new BoilerOperationException(ErrorMessages.InterlockMustBeClosed);
            }

            _boiler.Status = BoilerStatus.Ready;
            _boiler.CurrentCycle = null;
        }

        _eventLogService.Log("Boiler Status Changed", "Ready");
    }

    public void StartBoilerSequence()
    {
        lock (_boilerLock)
        {
            if (_boiler.Status != BoilerStatus.Ready)
            {
                throw new BoilerOperationException(ErrorMessages.BoilerNotReady);
            }

            _boiler.Status = BoilerStatus.Running;
            _boiler.CurrentCycle =
                new BoilerCycle
                {
                    StartTime = DateTime.Now,
                    Phase = BoilerPhase.PrePurge,
                    RemainingSeconds = 20,
                    ProgressPercentage = 0,
                };
        }

        _sequenceCts = new CancellationTokenSource();

        _sequenceTask = Task.Run(() => RunSequenceAsync(_sequenceCts.Token));
        _eventLogService.Log("Boiler Sequence", "Started");
    }


    public void StopBoilerSequence()
    {
        lock (_boilerLock)
        {
            if (_boiler.Status != BoilerStatus.Running)
            {
                throw new BoilerOperationException("Boiler is not running.");
            }

            _boiler.Status = BoilerStatus.Ready;
            _boiler.CurrentCycle = null;
        }

        _sequenceCts?.Cancel();
        _eventLogService.Log("Boiler Stopped", "Returned To Ready");
    }

    public void SimulateBoilerError()
    {
        lock (_boilerLock)
        {
            if(_boiler.Status != BoilerStatus.Operational)
            {
                throw new BoilerOperationException("Boiler is not in Operational State.");
            }

            _boiler.Status = BoilerStatus.Lockout;
            _boiler.CurrentCycle = null;
        }

        _sequenceCts?.Cancel();
        _eventLogService.Log("Simulate Boiler Error","Return to Lockout");
    }
    
    private async Task RunSequenceAsync(CancellationToken token)
    {
        try
        {
            await RunPrePurge(token);
            if (token.IsCancellationRequested)
            {
                return;
            }

            await RunIgnition(token);
            if (token.IsCancellationRequested)
            {
                return;
            }

            lock (_boilerLock)
            {
                _boiler.Status = BoilerStatus.Operational;
                _boiler.CurrentCycle = null;
            }

            _eventLogService.Log("Boiler Operational", "Running");
            SequenceCompleted?.Invoke(this, EventArgs.Empty);
        }
        catch (OperationCanceledException)
        {
        }
    }

    private async Task RunPrePurge(CancellationToken token)
    {
        using PeriodicTimer timer = new(TimeSpan.FromSeconds(1));
        for (int second = 10; second > 0; second--)
        {
            await timer.WaitForNextTickAsync(token);
            lock (_boilerLock)
            {
                if (_boiler.Status != BoilerStatus.Running)
                {
                    return;
                }

                _boiler.CurrentCycle!.Phase = BoilerPhase.PrePurge;
                _boiler.CurrentCycle.RemainingSeconds = second - 1 + 10;

                _boiler.CurrentCycle.ProgressPercentage = ((10 - second + 1) / 20.0) * 100;
            }

            ProgressChanged?.Invoke(this,
                new BoilerProgressEventArgs
                {
                    Phase = BoilerPhase.PrePurge,
                    RemainingSeconds = second - 1 + 10,
                    ProgressPercentage = ((10 - second + 1) / 20.0) * 100,
                });
        }

        _eventLogService.Log("Pre-Purge Completed", "Successful");
    }

    private async Task RunIgnition(CancellationToken token)
    {
        using PeriodicTimer timer = new(TimeSpan.FromSeconds(1));
        for (int second = 10; second > 0; second--)
        {
            await timer.WaitForNextTickAsync(token);
            lock (_boilerLock)
            {
                if (_boiler.Status != BoilerStatus.Running)
                {
                    return;
                }

                _boiler.CurrentCycle!.Phase = BoilerPhase.Ignition;
                _boiler.CurrentCycle.RemainingSeconds = second - 1;
                _boiler.CurrentCycle.ProgressPercentage = 50 + (((10 - second + 1) / 10.0) * 50);
            }

            ProgressChanged?.Invoke(this,
                new BoilerProgressEventArgs
                {
                    Phase = BoilerPhase.Ignition,
                    RemainingSeconds = second - 1,
                    ProgressPercentage = 50 + (((10 - second + 1) / 10.0) * 50)
                });
        }

        _eventLogService.Log("Ignition Completed", "Successful");
    }

    private static BoilerModel CopyBoiler(BoilerModel boiler)
    {
        return new BoilerModel
        {
            Status = boiler.Status,
            InterlockState = boiler.InterlockState,
            CurrentCycle = boiler.CurrentCycle is null ? null
                : new BoilerCycle
                {
                    StartTime = boiler.CurrentCycle.StartTime,
                    Phase = boiler.CurrentCycle.Phase,
                    RemainingSeconds = boiler.CurrentCycle.RemainingSeconds,
                    ProgressPercentage = boiler.CurrentCycle.ProgressPercentage
                }
        };
    }
}
