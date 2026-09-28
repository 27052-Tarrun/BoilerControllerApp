using BoilerController.Events;
using BoilerController.Models;

namespace BoilerController.Services;

/// <summary>
/// Provides boiler operations.
/// </summary>
public interface IBoilerService
{
    BoilerModel Boiler { get; }

    event EventHandler<BoilerProgressEventArgs>? ProgressChanged;

    event EventHandler? SequenceCompleted;

    void ToggleInterlock();

    void ResetLockout();

    void StartBoilerSequence();

    void StopBoilerSequence();

    void SimulateBoilerError();
}
