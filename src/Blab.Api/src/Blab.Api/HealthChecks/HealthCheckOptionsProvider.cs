using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace Blab.Api.HealthChecks;

/// <summary>
/// Type that can get a default set of <see cref="HealthCheckOptions"/>.
/// </summary>
internal static class HealthCheckOptionsProvider
{
    /// <summary>
    /// Gets the appropriate <see cref="HealthCheckOptions"/> based on the given <paramref name="configuration"/>.
    /// </summary>
    /// <param name="configuration">The <see cref="IConfiguration"/> object from which to create <see cref="HealthCheckOptions"/>.</param>
    /// <returns>A <see cref="HealthCheckOptions"/> object.</returns>
    internal static HealthCheckOptions CreateHealthCheckOptions(IConfiguration configuration)
    {
        var healthcheckConfig = configuration.GetSection("HealthcheckConfig").Get<HealthcheckConfig>();
        var options = new HealthCheckOptions();
        if (healthcheckConfig.EnableDetailedOutput)
        {
            options.ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse;
        }

        return options;
    }
}
