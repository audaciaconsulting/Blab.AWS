using Audacia.Auth.OpenIddict.Common.Configuration;

namespace Blab.Identity.Configuration.Mappers;

public class OpenIdConnectConfigMapper : IOpenIdConnectConfigMapper
{
    public OpenIdConnectConfig Map(IConfiguration configuration)
    {
        return configuration.GetSection(nameof(OpenIdConnectConfig)).Get<OpenIdConnectConfig>();
    }
}
