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
            if(_boiler.InterlockState == InterlockState.Open)
            {
                _boiler.Status = BoilerStatus.Lockout;
            }
        }

        _eventLogService.Log("Interlock Switch", $"Toggled To {_boiler.InterlockState}");
    }

    public void ResetLockout()
    {
        lock (_boilerLock)
        {
            if (_boiler.InterlockState == InterlockState.Open)
            {
                throw new BoilerOperationException(ErrorMessages.InterlockMustBeClosed);
            }

            _boiler.Status = BoilerStatus.Ready;
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
                    ProgressPercentage = 0
                };
        }

        _sequenceTask = Task.Run(RunSequenceAsync);
        _eventLogService.Log("Boiler Sequence", "Started");
    }

    private async Task RunSequenceAsync()
    {
        await RunPrePurge();
        await RunIgnition();

        lock (_boilerLock)
        {
            _boiler.Status = BoilerStatus.Operational;
            _boiler.CurrentCycle!.Phase = BoilerPhase.Operational;
            _boiler.CurrentCycle.RemainingSeconds = 0;
            _boiler.CurrentCycle.ProgressPercentage = 100;
        }

        _eventLogService.Log("Boiler Operational", "Running");
        SequenceCompleted?.Invoke(this, EventArgs.Empty);
    }

    private async Task RunPrePurge()
    {
        using PeriodicTimer timer = new(TimeSpan.FromSeconds(1));
        for (int second = 10; second > 0; second--)
        {
            await timer.WaitForNextTickAsync();
            double progress = ((double)(10 - second + 1) / 20) * 100;
            lock (_boilerLock)
            {
                _boiler.CurrentCycle!.Phase = BoilerPhase.PrePurge;
                _boiler.CurrentCycle.RemainingSeconds = second - 1 + 10;
                _boiler.CurrentCycle.ProgressPercentage = progress;
            }

            ProgressChanged?.Invoke(this,
                new BoilerProgressEventArgs
                {
                    Phase = BoilerPhase.PrePurge,
                    RemainingSeconds = second - 1 + 10,
                    ProgressPercentage = progress
                });
        }

        _eventLogService.Log("Pre-Purge Completed", "Successful");
    }

    private async Task RunIgnition()
    {
        using PeriodicTimer timer = new(TimeSpan.FromSeconds(1));
        for (int second = 10; second > 0; second--)
        {
            await timer.WaitForNextTickAsync();
            double progress = 50 + (((double)(10 - second + 1) / 10) * 50);
            lock (_boilerLock)
            {
                _boiler.CurrentCycle!.Phase = BoilerPhase.Ignition;
                _boiler.CurrentCycle.RemainingSeconds = second - 1;
                _boiler.CurrentCycle.ProgressPercentage = progress;
            }

            ProgressChanged?.Invoke(this,
                new BoilerProgressEventArgs
                {
                    Phase = BoilerPhase.Ignition,
                    RemainingSeconds = second - 1,
                    ProgressPercentage = progress
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
