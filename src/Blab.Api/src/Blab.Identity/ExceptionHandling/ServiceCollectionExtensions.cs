using Audacia.ExceptionHandling.AspNetCore;

namespace Blab.Identity.ExceptionHandling;

/// <summary>
/// Extensions to <see cref="IServiceCollection"/> related to exception handling.
/// </summary>
internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddExceptionHandlingServices(this IServiceCollection services) =>
        services.AddSingleton<IResponseSerializer, JsonResponseSerializer>();
}
