using OpenIddict.Validation.AspNetCore;

namespace Blab.Identity.Authentication;

/// <summary>
/// Extensions to <see cref="IServiceCollection"/> related to authentication.
/// </summary>
internal static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds authentication to the given <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to which to add authentication.</param>
    /// <returns>The given <paramref name="services"/>.</returns>
    internal static IServiceCollection AddAuthenticationServices(this IServiceCollection services) =>
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
            })
            // Add specific authentication config (such as token authority) in the call to AddJwtBearer
            .AddJwtBearer()
            .Services;
}
