using BoilerController.Enums;

namespace BoilerController.Models;

/// <summary>
/// Represents a boiler cycle.
/// </summary>
public class BoilerCycle
{
    /// <summary>
    /// Gets or sets StartTime of the cycle.
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// Gets or sets the boiler phase.
    /// </summary>
    public BoilerPhase Phase { get; set; }

    /// <summary>
    /// Gets or sets the progress percentage.
    /// </summary>
    public double ProgressPercentage { get; set; }

    /// <summary>
    /// Gets or sets the Remaining Seconds.
    /// </summary>
    public int RemainingSeconds { get; set; }
}
