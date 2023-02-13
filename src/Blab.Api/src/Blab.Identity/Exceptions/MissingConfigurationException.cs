namespace Blab.Identity.Exceptions;

/// <summary>
/// Exception class that represents an expected item missing in configuration.
/// </summary>
public class MissingConfigurationException : Exception
{
    public MissingConfigurationException()
    {
    }

    public MissingConfigurationException(string message)
        : base(message)
    {
    }

    public MissingConfigurationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
