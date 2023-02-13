namespace Blab.Domain.Common.Configuration;

/// <summary>
/// Config for all URL's of the different Apps apart of this solution.
/// </summary>
public class EndpointConfig
{
    /// <summary>
    /// Gets the Identifier for the section where the config options are stored within appsettings.json.
    /// </summary>
    public static string Location => "EndpointConfig";

    /// <summary>
    /// Gets or sets the URL of where the API is located.
    /// </summary>
    public Uri? Api { get; set; }

    /// <summary>
    /// Gets or sets the URL of where the Identity Server is located.
    /// </summary>
    public Uri? Identity { get; set; }

    /// <summary>
    /// Gets or sets the URL of where the UI is located.
    /// </summary>
    public Uri? Ui { get; set; }
}
