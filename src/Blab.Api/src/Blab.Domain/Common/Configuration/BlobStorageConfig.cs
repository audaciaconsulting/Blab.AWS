
namespace Blab.Api.Configuration.Options;

/// <summary>
/// Blob storage config.
/// </summary>
public class BlobStorageConfig
{
    /// <summary>
    /// Gets or sets the Profile pictures container url.
    /// </summary>
    public Uri? ProfileContainerUrl { get; set; }

    /// <summary>
    /// Gets or sets the background pictures container url.
    /// </summary>
    public Uri? BackgroundProfileContainerUrl { get; set; }

    /// <summary>
    /// Gets or sets the name of the Azure Blob storage account.
    /// </summary>
    public string AccountName { get; set; }

    /// <summary>
    /// Gets or sets the key of the Azure Blob Storage account.
    /// </summary>
    public string AccountKey { get; set; }
}
