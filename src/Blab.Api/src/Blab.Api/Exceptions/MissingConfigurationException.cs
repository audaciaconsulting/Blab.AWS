namespace Blab.Api.Exceptions;

/// <summary>
/// Exception class that represents an expected item missing in configuration.
/// </summary>
public class MissingConfigurationException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MissingConfigurationException"/> class.
    /// </summary>
    public MissingConfigurationException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MissingConfigurationException"/> class.
    /// </summary>
    /// <param name="message">The exception message.</param>
    public MissingConfigurationException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MissingConfigurationException"/> class.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="innerException">The inner exception.</param>
    public MissingConfigurationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
