namespace Blab.Identity.HealthChecks;

/// <summary>
/// Represents the "HealthcheckConfig" section of configuration.
/// </summary>
public class HealthcheckConfig
{
    /// <summary>
    /// Gets or sets a value indicating whether or not detailed output should be provided as part of the health check response.
    /// </summary>
    public bool EnableDetailedOutput { get; set; }
}
