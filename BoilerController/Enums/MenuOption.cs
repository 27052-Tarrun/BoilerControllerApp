namespace BoilerController.Enums;

/// <summary>
/// Represents Menu Options.
/// </summary>
public enum MenuOption
{
    /// <summary>
    /// Exit Option.
    /// </summary>
    Exit = 0,

    /// <summary>
    /// Toggle Interlock Switch Option.
    /// </summary>
    ToggleRunInterlockSwitch = 1,

    /// <summary>
    /// Reset Lockout Option.
    /// </summary>
    ResetLockout = 2,

    /// <summary>
    /// Start Boiler Sequence Option.
    /// </summary>
    StartBoilerSequence = 3,

    /// <summary>
    /// Exit Option.
    /// </summary>
    StopBoilerSequence = 4,

    /// <summary>
    /// Simulate Boiler Error Option.
    /// </summary>
    SimulateBoilerError = 5,

    /// <summary>
    /// View Event Log Option.
    /// </summary>
    ViewEventLog = 6,
}
