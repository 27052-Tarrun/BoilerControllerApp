namespace BoilerController.Exceptions;

/// <summary>
/// Custom Exception used in the application.
/// </summary>
public class StorageException : Exception
{
    public StorageException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
