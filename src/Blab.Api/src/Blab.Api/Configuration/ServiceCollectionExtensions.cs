using System.Configuration;
using Audacia.Azure.BlobStorage.Config;
using Blab.Api.Configuration.Options;
using Blab.Domain.Common.Configuration;

namespace Blab.Api.Configuration;

/// <summary>
/// Service Collection extensions for adding config options to the DI container.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adding all the configuration options to Dependency Injection container.
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddConfiguration(
        this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        _ = configuration ?? throw new ConfigurationErrorsException(nameof(configuration));

        return serviceCollection
            .Configure<EndpointConfig>(configuration.GetSection(EndpointConfig.Location))
            .Configure<AuthenticationConfig>(configuration.GetSection(AuthenticationConfig.Location))
            .Configure<BlobStorageOption>(configuration.GetSection(BlobStorageOption.OptionConfigLocation))
            .Configure<BlobStorageConfig>(configuration.GetSection(BlobStorageOption.OptionConfigLocation));
    }
}
