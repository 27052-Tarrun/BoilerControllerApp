
namespace BoilerController.Enums;

/// <summary>
/// Represents current boiler phase.
/// </summary>
public enum BoilerPhase
{
    /// <summary>
    /// Boiler is in PrePurge state.
    /// </summary>
    PrePurge,

    /// <summary>
    /// Boiler is in Ignition state.
    /// </summary>
    Ignition,

    /// <summary>
    /// Boiler is in Operational state.
    /// </summary>
    Operational,
}
