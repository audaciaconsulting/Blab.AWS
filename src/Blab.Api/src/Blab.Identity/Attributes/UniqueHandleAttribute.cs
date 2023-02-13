using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Blab.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Blab.Identity.Attributes;

/// <summary>
/// Validation attribute to assert a handle is unique.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
    AllowMultiple = false)]
public class UniqueHandleAttribute : ValidationAttribute
{
    /// <summary>
    /// Checks if the input handle already exists in the system.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="validationContext"></param>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is string)
        {
            var context = (BlabDbContext)validationContext
                .GetService(typeof(BlabDbContext));
            if (context.Users.AsNoTracking().Any(user => user.UserName == value.ToString()))
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }

        return new ValidationResult($"{validationContext.DisplayName} is not a string.");
    }

    public override string FormatErrorMessage(string name)
    {
        string errorMessage = "The {0} is already taken.";
        return string.Format(CultureInfo.CurrentCulture, errorMessage, name);
    }
}
