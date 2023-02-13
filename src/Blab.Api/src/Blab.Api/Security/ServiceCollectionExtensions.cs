using Audacia.Core;

namespace Blab.Api.Security;

/// <summary>
/// Extensions to <see cref="IServiceCollection"/> related to security.
/// </summary>
internal static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds security-related services to the given <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to which to add the services.</param>
    /// <returns>The given <paramref name="services"/>.</returns>
    internal static IServiceCollection AddSecurityServices(this IServiceCollection services) =>
        services.AddTransient<ICurrentUserProvider<int?>>(provider =>
            new ClaimCurrentUserProvider(provider.GetRequiredService<IHttpContextAccessor>(), ClaimTypes.UserId));
}
