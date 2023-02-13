using Blab.Api.Exceptions;
using Blab.Domain.Common.Configuration;

namespace Blab.Api.Cors;

/// <summary>
/// Extensions to <see cref="IServiceCollection"/> related to Cors.
/// </summary>
internal static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds Cors to the given <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> object to which to add CORS services.</param>
    /// <param name="configuration">The current <see cref="IConfiguration"/> object.</param>
    /// <returns>The given <paramref name="services"/>.</returns>
    /// <exception cref="MissingConfigurationException">If <see cref="EndpointConfig"/> can't be found in the given <paramref name="configuration"/>.</exception>
    internal static IServiceCollection AddCorsServices(this IServiceCollection services, IConfiguration configuration)
    {
        var endpointConfig = configuration.GetSection("EndpointConfig").Get<EndpointConfig>();
        if (endpointConfig?.Ui is null)
        {
            throw new MissingConfigurationException(
                "The 'Ui' property could be read from the 'EndpointConfig' configuration section.");
        }

        return services.AddCors(options =>
        {
            options.AddPolicy(CorsPolicies.Default, builder =>
            {
                builder.WithOrigins(endpointConfig.Ui.OriginalString);
                builder.WithHeaders("authorization", "content-type", "connection", "host", "X-Requested-With", "Refer",
                    "Origin");
                builder.WithMethods(HttpMethods.Get, HttpMethods.Post, HttpMethods.Put, HttpMethods.Delete,
                    HttpMethods.Options);
                builder.AllowCredentials();
            });
        });
    }
}
