using Audacia.Core;
using Blab.Api.Authentication;
using Blab.Api.Exceptions;
using Blab.Api.Security;
using Blab.Domain.Common.Configuration;
using Blab.Domain.Entities.Security;
using Blab.Services.Services.Security.Handlers;
using Blab.Services.Services.Security.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Blab.Api.IoC.ApplicationServices;

/// <summary>
/// Service Collection extensions for Application services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds all the application services to the IoC container for Blab.Api.
    /// </summary>
    /// <param name="services">Instance of <see cref="IServiceCollection"/>.</param>
    /// <param name="configuration">Instance of <see cref="IConfiguration"/>.</param>
    /// <returns></returns>
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddSecurityServices()
            .AddHttpClientServices(configuration);

        return services;
    }

    /// <summary>
    /// Adds security-related services to the given <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to which to add the services.</param>
    /// <returns>The given <paramref name="services"/>.</returns>
    private static IServiceCollection AddSecurityServices(this IServiceCollection services)
    {
        services.AddScoped<UserManager<ApplicationUser>>()
            .AddTransient<ICurrentUserProvider<int?>>(provider =>
                new ClaimCurrentUserProvider(provider.GetRequiredService<IHttpContextAccessor>(), ClaimTypes.UserId));

        return services;
    }

    /// <summary>
    /// Adds the Http Client services. 
    /// </summary>
    /// <param name="services">Instance of <see cref="IServiceCollection"/>.</param>
    /// <param name="configuration">Instance of <see cref="IConfiguration"/>.</param>
    /// <returns></returns>
    /// <exception cref="MissingConfigurationException">
    /// Thrown when their is missing configuration from appsettings.json.
    /// </exception>
    private static IServiceCollection AddHttpClientServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var httpBuilder = services
            .AddTransient<IdentityAuthenticationHttpClientHandler>()
            .AddHttpClient<IGetResetTokenHelperService, GetResetTokenHelperService>();

        var endpointConfig = configuration.GetSection("EndpointConfig").Get<EndpointConfig>();
        if (endpointConfig?.Identity is null)
        {
            throw new MissingConfigurationException(
                "The 'Identity' property could be read from the 'EndpointConfig' configuration section.");
        }

        httpBuilder.ConfigureHttpClient((_, c) => c.BaseAddress = endpointConfig.Identity);

        httpBuilder
            .AddHttpMessageHandler<IdentityAuthenticationHttpClientHandler>();

        return services;
    }
}
