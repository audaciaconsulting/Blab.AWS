using Blab.Api.Exceptions;
using Blab.Domain.Common.Configuration;
using Microsoft.OpenApi.Models;

namespace Blab.Api.Swagger;

/// <summary>
/// Extensions to <see cref="IServiceCollection"/> related to Swagger.
/// </summary>
internal static class ServiceCollectionExtensions
{
    private const string SecurityDefinitionName = "Blab API";
    
    /// <summary>
    /// Adds Swagger generation to the given <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> instance to which to add Swagger.</param>
    /// <param name="configuration"></param>
    /// <returns>The given <paramref name="services"/> with Swagger added.</returns>
    internal static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        var endpointConfig = configuration.GetSection(EndpointConfig.Location).Get<EndpointConfig>();
        if (endpointConfig?.Identity is null)
        {
            throw new MissingConfigurationException(
                "The 'Identity' property could be read from the 'EndpointConfig' configuration section.");
        }

        return services.AddSwaggerGen(options =>
        {
            options.UseInlineDefinitionsForEnums();
            options.AddSecurityDefinition(SecurityDefinitionName, new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Name = "Authorization",
                In = ParameterLocation.Header,
                Scheme = "Bearer",
                Flows = new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri($"{endpointConfig.Identity}connect/authorize"),
                        TokenUrl = new Uri($"{endpointConfig.Identity}connect/token"),
                        Scopes = new Dictionary<string, string>
                        {
                            {
                                "api", "api"
                            }
                        }
                    }
                },
                OpenIdConnectUrl = new Uri($"{endpointConfig.Identity}.well-known/openid-configuration")
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = SecurityDefinitionName
                        }
                    },
                    new List<string> { "api" }
                }
            });
        });
    }
}
