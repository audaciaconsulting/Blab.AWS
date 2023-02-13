using Microsoft.AspNetCore.Rewrite;

namespace Blab.Identity.HealthChecks;

/// <summary>
/// Extensions to <see cref="IApplicationBuilder"/> related to health check url rewrites.
/// </summary>
internal static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Adds a rule to rewrite requests to the root url to /health.
    /// </summary>
    /// <param name="applicationBuilder">The <see cref="IApplicationBuilder"/> to which to add the rewrite.</param>
    /// <returns>The given <paramref name="applicationBuilder"/>.</returns>
    internal static IApplicationBuilder UseHealthCheckUrlRewrite(this IApplicationBuilder applicationBuilder)
    {
        var rewriteOptions = new RewriteOptions();
        rewriteOptions.AddRewrite("^$", "health", true);

        return applicationBuilder.UseRewriter(rewriteOptions);
    }
}
