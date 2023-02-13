namespace Blab.Services.Models.Validation;

public class PropertyErrorMessage : IError
{
    public string PropertyName { get; }

    public string ErrorMessage { get; }

    public PropertyErrorMessage(string propertyName, string errorMessage)
    {
        PropertyName = propertyName;
        ErrorMessage = errorMessage;
    }
}
