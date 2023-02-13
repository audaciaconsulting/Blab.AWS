using Microsoft.AspNetCore.Authorization;

namespace Blab.Identity.Authorization;

/// <summary>
/// Extensions to <see cref="IServiceCollection"/> related to authorization.
/// </summary>
internal static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds authorization with policies to the given <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to which to add authorization policies.</param>
    /// <returns>The given <paramref name="services"/>.</returns>
    internal static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services) =>
        services.AddAuthorization(options =>
        {
            // Will require authenticated users to all endpoints by default
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        });
}
