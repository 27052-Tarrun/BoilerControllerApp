using BoilerController.Enums;

namespace BoilerController.Models;

/// <summary>
/// Represents a boiler.
/// </summary>
public class BoilerModel
{
    /// <summary>
    /// Gets or sets current boiler status.
    /// </summary>
    public BoilerStatus Status { get; set; }

    /// <summary>
    /// Gets or sets current interlock state.
    /// </summary>
    public InterlockState InterlockState { get; set; }

    /// <summary>
    /// Gets or sets running cycle information.
    /// </summary>
    public BoilerCycle? CurrentCycle { get; set; }
}
