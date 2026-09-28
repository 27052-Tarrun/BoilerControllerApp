using BoilerController.Events;
using BoilerController.Models;

namespace BoilerController.Services;

/// <summary>
/// Provides boiler operations.
/// </summary>
public interface IBoilerService
{
    /// <summary>
    /// Gets the current boiler state.
    /// </summary>
    BoilerModel Boiler { get; }

    /// <summary>
    /// Raised when boiler sequence progress changes.
    /// </summary>
    event EventHandler<BoilerProgressEventArgs>? ProgressChanged;

    /// <summary>
    /// Raised when the boiler sequence completes.
    /// </summary>
    event EventHandler? SequenceCompleted;

    /// <summary>
    /// Toggles the run interlock switch.
    /// </summary>
    void ToggleInterlock();

    /// <summary>
    /// Resets boiler lockout state.
    /// </summary>
    void ResetLockout();

    /// <summary>
    /// Starts boiler startup sequence.
    /// </summary>
    void StartBoilerSequence();

    /// <summary>
    /// Stops the boiler sequence.
    /// </summary>
    void StopBoilerSequence();

    /// <summary>
    /// Simulate an error on Operational state.
    /// </summary>
    void SimulateBoilerError();
}
