using Blab.Services.Models.Validation;
using FluentValidation.Results;

namespace Blab.Services.Extensions.FluentValidationExtensions;

public static class ValidationFailures
{
    /// <summary>
    /// Returns Fluent Validation error messages as <see cref="IError"/>'s.
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static IEnumerable<IError> ErrorMessages(this IEnumerable<ValidationFailure> source) =>
        source.Select(validationFailure =>
                new PropertyErrorMessage(validationFailure.PropertyName, validationFailure.ErrorMessage))
            .ToList();
}
