namespace Blab.Api.Configuration.Options;

/// <summary>
/// Configuration for setting up Authentication for OpenIddict.
/// </summary>
public class AuthenticationConfig
{
    /// <summary>
    /// Gets the Identifier for the section where the config options are stored within appsettings.json.
    /// </summary>
    public static string Location => "AuthenticationConfig";

    /// <summary>
    /// Gets or sets the Id used to handshake with Blab.Identity.
    /// </summary>
    public string? ClientId { get; set; }

    /// <summary>
    /// Gets or sets the Secret used to handshake with Blab.Identity.
    /// </summary>
    public string? ClientSecret { get; set; }
}
