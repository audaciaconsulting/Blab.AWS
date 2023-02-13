namespace Blab.Api.ExceptionHandling;

/// <summary>
/// Defines the context for exception that has been handled to the UI.
/// </summary>
public static class ExceptionResponseTypes
{
    /// <summary>
    /// A domain exception.
    /// </summary>
    public const string Domain = "Domain";

    /// <summary>
    /// A validation exception.
    /// </summary>
    public const string Validation = "Validation";
}
