namespace BoilerController.Enums;

/// <summary>
/// Represents current boiler status.
/// </summary>
public enum BoilerStatus
{
    /// <summary>
    /// Boiler is locked out.
    /// </summary>
    Lockout,

    /// <summary>
    /// Boiler is ready to start.
    /// </summary>
    Ready,

    /// <summary>
    /// Boiler startup sequence is running.
    /// </summary>
    Running,

    /// <summary>
    /// Boiler is operational.
    /// </summary>
    Operational,
}
