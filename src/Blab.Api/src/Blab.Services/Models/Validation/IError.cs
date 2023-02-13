namespace Blab.Services.Models.Validation;

/// <summary>
/// Interface for passing errors to the client allowing the Front end to match against the property.
/// </summary>
public interface IError
{
    /// <summary>
    /// The property on the front end which the <see cref="ErrorMessage"/> will appear against.
    /// </summary>
    string PropertyName { get; }

    /// <summary>
    /// Description of the error.
    /// </summary>
    string ErrorMessage { get; }
}
