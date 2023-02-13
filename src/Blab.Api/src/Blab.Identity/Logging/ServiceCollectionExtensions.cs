using Audacia.Log.AspNetCore;

namespace Blab.Identity.Logging;

/// <summary>
/// Extensions to <see cref="IServiceCollection"/> related to logging.
/// </summary>
internal static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds logging services to the given <see cref="IServiceCollection"/> using the <see cref="Audacia.Log.AspNetCore"/> library.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> object to which to add logging.</param>
    /// <param name="configuration">The current <see cref="IConfiguration"/>.</param>
    /// <returns>The given <paramref name="services"/>.</returns>
    internal static IServiceCollection AddLoggingServices(this IServiceCollection services,
        IConfiguration configuration) =>
        // Add additional logging configuration here: see https://dev.azure.com/audacia/Audacia/_git/Audacia.Log?path=/Audacia.Log.AspNetCore
        services.ConfigureApplicationInsights(configuration);
}
