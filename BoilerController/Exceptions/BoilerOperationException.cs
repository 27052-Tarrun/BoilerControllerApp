namespace BoilerController.Exceptions;

public class BoilerOperationException : Exception
{
    public BoilerOperationException(string message)
        : base(message)
    {
    }
}
