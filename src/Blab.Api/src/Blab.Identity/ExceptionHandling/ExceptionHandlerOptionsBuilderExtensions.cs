using Audacia.ExceptionHandling;

namespace Blab.Identity.ExceptionHandling;

/// <summary>
/// Extensions to the <see cref="ExceptionHandlerOptionsBuilder"/> type.
/// </summary>
internal static class ExceptionHandlerOptionsBuilderExtensions
{
    /// <summary>
    /// Handles a <see cref="BaseDomainException"/> and returns a BadRequest containing the validation errors.
    /// </summary>
    internal static ExceptionHandlerOptionsBuilder DomainException(this ExceptionHandlerOptionsBuilder builder)
    {
        // Uncomment when a specific domain exception type is available, i.e. when a domain project exists
        //return builder.Handle(
        //    (BaseDomainException exception) => ErrorResult.FromException(exception),
        //    statusCode: HttpStatusCode.BadRequest,
        //    responseType: ExceptionResponseTypes.Domain);

        return builder;
    }
}
