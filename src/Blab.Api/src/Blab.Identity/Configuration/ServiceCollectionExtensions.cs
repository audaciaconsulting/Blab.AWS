using Audacia.Auth.OpenIddict.Common.Configuration;
using Blab.Domain.Common.Configuration;

namespace Blab.Identity.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConfigOptions(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        return serviceCollection.Configure<OpenIdConnectConfig>(configuration.GetSection(nameof(OpenIdConnectConfig)))
            .Configure<EndpointConfig>(configuration.GetSection(nameof(EndpointConfig)));
    }
}
