using Blab.Api.ExceptionHandling;

namespace Blab.Api.Mvc;

/// <summary>
/// Extensions to <see cref="IServiceCollection"/> related to Mvc.
/// </summary>
internal static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds Mvc services to the given <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to which to add Mvc services.</param>
    /// <returns>The given <paramref name="services"/>.</returns>
    internal static IServiceCollection AddMvcServices(this IServiceCollection services)
    {
        return services
            .AddControllers()
            .ConfigureApiBehaviorOptions(options =>
            {
                // Convert ModelState Errors into DomainValidationErrors
                options.InvalidModelStateResponseFactory = context =>
                {
                    var loggerFactory = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>();

                    return context.ModelState.AsValidationResponse(loggerFactory);
                };
            })
            .Services;
    }
}
