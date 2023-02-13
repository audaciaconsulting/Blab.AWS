namespace Blab.Identity.HealthChecks;

/// <summary>
/// Extensions to <see cref="IHealthChecksBuilder"/>.
/// </summary>
internal static class HealthChecksBuilderExtensions
{
    /// <summary>
    /// Configures additional health check services on the given <paramref name="healthChecksBuilder"/>.
    /// </summary>
    /// <param name="healthChecksBuilder">The <see cref="IHealthChecksBuilder"/> instance on which to configure services.</param>
    /// <returns>An <see cref="IServiceCollection"/> instance.</returns>
    internal static IServiceCollection ConfigureHealthChecks(this IHealthChecksBuilder healthChecksBuilder)
    {
        // This is where you should add additional health checks for things like database connections
        return healthChecksBuilder.Services;
    }
}
