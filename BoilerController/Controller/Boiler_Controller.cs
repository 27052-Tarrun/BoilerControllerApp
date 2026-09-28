using BoilerController.Constants;
using BoilerController.Enums;
using BoilerController.Events;
using BoilerController.Models;
using BoilerController.Services;
using BoilerController.Views;

namespace BoilerController.Controller;

/// <summary>
/// Controls application flow.
/// </summary>
public class Boiler_Controller : IBoilerController
{
    private readonly DashboardView _dashboardView = new();

    private readonly IBoilerService _boilerService;
    private readonly IEventLogService _eventLogService;

    private bool _isRunning = true;

    private CancellationTokenSource? _dashboardRefreshCts;
    private Task? _dashboardRefreshTask;

    private bool _sequenceCompleted = false;

    public Boiler_Controller(IBoilerService boilerService, IEventLogService eventLogService)
    {
        _boilerService = boilerService;
        _eventLogService = eventLogService;
    }

    /// <inheritdoc/>
    public void Start()
    {
        Console.WriteLine(CommonMessages.ControllerInitialized);
        Thread.Sleep(1000);

        _boilerService.ProgressChanged += OnProgressChanged;
        _boilerService.SequenceCompleted += OnSequenceCompleted;
        StartDashboardRefresh();

        _dashboardView.EnsureRendererInitialized(_boilerService.Boiler);

        while (_isRunning)
        {
            MenuOption option = _dashboardView.Show(_boilerService.Boiler);
            HandleMenuOption(option);
        }

        StopDashboardRefresh();
    }

    private void HandleMenuOption(MenuOption option)
    {
        switch (option)
        {
            case MenuOption.ToggleRunInterlockSwitch:
                ToggleInterlock();
                break;

            case MenuOption.ResetLockout:
                ResetLockout();
                break;

            case MenuOption.StartBoilerSequence:
                StartBoilerSequence();
                break;

            case MenuOption.StopBoilerSequence:
                StopBoilerSequence();
                break;

            case MenuOption.SimulateBoilerError:
                SimulateBoilerError();
                break;

            case MenuOption.ViewEventLog:
                ViewEventLog();
                break;            

            case MenuOption.Exit:
                _isRunning = false;
                break;

            default:

                _dashboardView.DisplayError(ErrorMessages.InvalidMenuChoice);
                break;
        }
    }

    private void ToggleInterlock()
    {
        try
        {
            _boilerService.ToggleInterlock();
            _dashboardView.DisplayMessageWithRedraw(_boilerService.Boiler, "Interlock Switch Toggled.");
        }
        catch (Exception ex)
        {
            _dashboardView.DisplayErrorWithRedraw(_boilerService.Boiler, ex.Message);
        }
    }

    private void ResetLockout()
    {
        try
        {
            _boilerService.ResetLockout();
            _dashboardView.DisplayMessageWithRedraw(_boilerService.Boiler, "Boiler Ready.");
        }
        catch (Exception ex)
        {
            _dashboardView.DisplayErrorWithRedraw(_boilerService.Boiler, ex.Message);
        }
    }

    private void StartBoilerSequence()
    {
        try
        {
            _boilerService.StartBoilerSequence();
            _dashboardView.DisplayMessageWithRedraw(_boilerService.Boiler, "Boiler Sequence Started.");
        }
        catch (Exception ex)
        {
            _dashboardView.DisplayErrorWithRedraw(_boilerService.Boiler, ex.Message);
        }
    }

    private void StopBoilerSequence()
    {
        try
        {
            _boilerService.StopBoilerSequence();
            _dashboardView.DisplayMessageWithRedraw(_boilerService.Boiler, "Boiler Sequence Stopped.");
        }
        catch (Exception ex)
        {
            _dashboardView.DisplayErrorWithRedraw(_boilerService.Boiler, ex.Message);
        }
    }

    private void SimulateBoilerError()
    {
        try
        {
            _boilerService.SimulateBoilerError();
            _dashboardView.DisplayMessageWithRedraw(_boilerService.Boiler, "Boiler Error Simulated.");
        }
        catch (Exception ex)
        {
            _dashboardView.DisplayErrorWithRedraw(_boilerService.Boiler,ex.Message);
        }
    }

    private void ViewEventLog()
    {
        EventLogView eventLogView = new(_dashboardView.GetRenderer());

        eventLogView.DisplayLogs(_eventLogService.GetAll());
        Console.Clear();
        _dashboardView.ForceFullRedraw(_boilerService.Boiler);
    }

    private void StartDashboardRefresh()
    {
        _dashboardRefreshCts = new CancellationTokenSource();
        CancellationToken token = _dashboardRefreshCts.Token;
        _dashboardRefreshTask =
            Task.Run(async () =>
            {
                using PeriodicTimer timer = new(TimeSpan.FromSeconds(1));
                try
                {
                    while (await timer.WaitForNextTickAsync(token))
                    {
                        BoilerModel boiler = _boilerService.Boiler;
                        if (boiler.Status == BoilerStatus.Running || boiler.Status == BoilerStatus.Operational)
                        {
                            _dashboardView.UpdateDashboard(boiler);
                        }
                    }
                }
                catch
                {
                }
            });
    }

    private void StopDashboardRefresh()
    {
        _dashboardRefreshCts?.Cancel();
        try
        {
            _dashboardRefreshTask?.GetAwaiter().GetResult();
        }
        catch
        {
        }

        _dashboardRefreshCts?.Dispose();
        _dashboardRefreshTask = null;
        _dashboardRefreshCts = null;
    }

    private void OnProgressChanged(object? sender, BoilerProgressEventArgs e)
    {
        
    }

    private void OnSequenceCompleted(object? sender, EventArgs e)
    {
        _sequenceCompleted = true;
    }
}