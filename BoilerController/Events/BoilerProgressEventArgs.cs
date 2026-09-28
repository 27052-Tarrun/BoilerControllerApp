using BoilerController.Enums;

namespace BoilerController.Events;

/// <summary>
/// Provides boiler sequence progress information.
/// </summary>
public class BoilerProgressEventArgs : EventArgs
{
    /// <summary>
    /// Gets or sets current boiler phase.
    /// </summary>
    public BoilerPhase Phase { get; set; }

    /// <summary>
    /// Gets or sets progress percentage.
    /// </summary>
    public double ProgressPercentage { get; set; }

    /// <summary>
    /// Gets or sets remaining time in seconds.
    /// </summary>
    public int RemainingSeconds { get; set; }
}
