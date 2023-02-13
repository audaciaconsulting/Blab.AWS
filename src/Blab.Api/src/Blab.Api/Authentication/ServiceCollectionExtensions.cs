using Blab.Api.Configuration.Options;
using Blab.Api.Exceptions;
using Blab.Domain.Common.Configuration;
using OpenIddict.Validation.AspNetCore;

namespace Blab.Api.Authentication;

/// <summary>
/// Extensions to <see cref="IServiceCollection"/> related to authentication.
/// </summary>
internal static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds authentication to the given <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to which to add authentication.</param>
    /// <param name="configuration"></param>
    /// <returns>The given <paramref name="services"/>.</returns>
    internal static IServiceCollection AddAuthenticationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var (endpointConfig, authenticationConfig) = ValidateConfig(configuration);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
        });

        services.AddOpenIddict()
            .AddValidation(options => AddOpenIddictValidation(options, endpointConfig, authenticationConfig));

        return services;
    }

    private static void AddOpenIddictValidation(
        OpenIddictValidationBuilder builder,
        EndpointConfig endpointConfig,
        AuthenticationConfig authenticationConfig)
    {
        builder.SetIssuer(endpointConfig.Identity?.OriginalString ?? string.Empty);
        builder.AddAudiences(authenticationConfig.ClientId ?? string.Empty);

        builder
            .UseIntrospection()
            // To allow the below, the API will need to be registered as a 'client credentials' client with OpenIddict if it isn't already
            .SetClientId(authenticationConfig.ClientId ?? string.Empty)
            .SetClientSecret(authenticationConfig.ClientSecret ?? string.Empty);

        builder.UseSystemNetHttp();
        builder.UseAspNetCore();
    }

    /// <summary>
    /// Validates if all configuration has being set up correctly.
    /// </summary>
    /// <param name="configuration"></param>
    /// <returns></returns>
    /// <exception cref="MissingConfigurationException"></exception>
    private static (EndpointConfig EndpointConfig, AuthenticationConfig AuthenticationConfig) ValidateConfig(
        IConfiguration configuration)
    {
        var endpointConfig = configuration.GetSection(EndpointConfig.Location).Get<EndpointConfig>();
        if (endpointConfig?.Identity is null)
        {
            throw new MissingConfigurationException(
                "The 'Identity' property could be read from the 'EndpointConfig' configuration section.");
        }

        var authenticationConfig = configuration.GetSection(AuthenticationConfig.Location).Get<AuthenticationConfig>();
        if (authenticationConfig?.ClientId is null || authenticationConfig?.ClientSecret is null)
        {
            throw new MissingConfigurationException(
                "The 'ClientId' or 'ClientSecret' property could be read from the 'AuthenticationConfig' configuration section.");
        }

        return (endpointConfig, authenticationConfig);
    }
}
