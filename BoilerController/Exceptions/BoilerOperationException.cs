namespace BoilerController.Exceptions;

/// <summary>
/// Custom Exception used in the application.
/// </summary>
public class BoilerOperationException : Exception
{
    public BoilerOperationException(string message)
        : base(message)
    {
    }
}
