using Audacia.ExceptionHandling.AspNetCore;

namespace Blab.Identity.ExceptionHandling;

internal static class ApplicationBuilderExtensions
{
    internal static IApplicationBuilder
        UseExceptionHandling(this IApplicationBuilder app, ILoggerFactory loggerFactory) =>
        app.ConfigureExceptions(loggerFactory, builder =>
        {
            // Add handling for custom exception types here, e.g. handle domain exceptions as bad requests
            builder
                .DomainException()
                .WithDefaultLogging((ILogger logger, Exception exception) =>
                    logger.LogError(exception, exception.Message));
        });
}
