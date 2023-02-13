using System.Net.Mime;
using Audacia.ExceptionHandling.Extensions;
using Audacia.ExceptionHandling.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Blab.Api.ExceptionHandling;

/// <summary>
/// Extensions to the <see cref="ModelStateDictionary"/> type.
/// </summary>
internal static class ModelStateDictionaryExtensions
{
    /// <summary>
    /// Converts a <see cref="ModelStateDictionary"/> into a BadRequest containing the validation errors.
    /// </summary>
    public static BadRequestObjectResult AsValidationResponse(this ModelStateDictionary modelState, ILoggerFactory loggerFactory)
    {
        // Group individual validation errors by Field Name
        var validationErrors = modelState.Keys.Select(fieldName =>
        {
            var fieldErrors = modelState[fieldName]?.Errors?.Select(error => error.ErrorMessage).ToArray();

            return new ErrorResult(fieldName, fieldErrors ?? Array.Empty<string>());
        });

        var result = validationErrors.CreateErrorResponse(loggerFactory, ExceptionResponseTypes.Validation);
        var response = new BadRequestObjectResult(result);
        response.ContentTypes.Add(MediaTypeNames.Application.Json);

        return response;
    }
}
