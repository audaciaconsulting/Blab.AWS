using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Blab.Identity.Attributes;

/// <summary>
/// Validation attribute to assert the age of the user meets the minimum age requirements.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
    AllowMultiple = false)]
public class MinimumAgeAttribute : ValidationAttribute
{
    /// <summary>
    /// Constructor that accepts the minimum age required.
    /// </summary>
    /// <param name="age"></param>
    public MinimumAgeAttribute(int age)
        => MinAge = age;

    /// <summary>
    /// Gets the minimum age to check against.
    /// </summary>
    public int MinAge { get; }

    /// <summary>
    /// Checks the age of the user meets the minimum age requirement.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="validationContext"></param>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        DateTime dateOfBirth;
        if (value is not null && DateTime.TryParse(value.ToString(), out dateOfBirth))
        {
            if (dateOfBirth >= DateTime.Now.AddYears(-MinAge))
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }

        return new ValidationResult($"{validationContext.DisplayName} is not a date.");
    }

    /// <summary>
    /// Override of <see cref="ValidationAttribute.FormatErrorMessage" />
    /// </summary>
    /// <param name="name"></param>
    public override string FormatErrorMessage(string name)
    {
        string errorMessage = "Cannot be under the age of {0}";
        return string.Format(CultureInfo.CurrentCulture, errorMessage, MinAge);
    }
}
