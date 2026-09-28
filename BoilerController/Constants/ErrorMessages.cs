namespace BoilerController.Constants;

/// <summary>
/// Provides Error messages used accross the application.
/// </summary>
public static class ErrorMessages
{
    public const string InvalidMenuChoice = "Invalid Menu Choice.";

    public const string InterlockMustBeClosed = "Close The Run Interlock Switch First.";

    public const string BoilerNotReady = "Boiler Must Be In Ready State.";

    public const string BoilerAlreadyRunning = "Boiler Sequence Already Running.";
}