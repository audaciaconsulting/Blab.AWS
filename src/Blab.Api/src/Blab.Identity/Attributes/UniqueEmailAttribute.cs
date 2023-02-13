using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Blab.DataAccess;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Manage.Internal;
using Microsoft.EntityFrameworkCore;
using EmailModel = Microsoft.AspNetCore.Identity.UI.V5.Pages.Account.Manage.Internal.EmailModel;

namespace Blab.Identity.Attributes;

/// <summary>
/// Validation attribute to assert a handle is unique.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
    AllowMultiple = false)]
public class UniqueEmailAttribute : ValidationAttribute
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
            if (context.Users.AsNoTracking().Any(user => user.Email == value.ToString()))
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