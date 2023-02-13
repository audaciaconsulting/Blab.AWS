using Audacia.Azure.Common.ReturnOptions.ImageOption;
using Blab.Domain.Entities.Security;

namespace Blab.Domain.Models.Photo;

/// <summary>
/// This is the Background Photo class inherits the propertys of the IPhoto interface.
/// </summary>
public class BackgroundPhoto : IPhoto
{
    /// <summary>
    /// Gets or sets the id of the background photo.
    /// This is set in the SQL database,
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the userId of the user that background photo belongs to.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets the navigational property of the user.
    /// </summary>
    public ApplicationUser User { get; set; }

    /// <summary>
    /// Gets or sets the BlobName of background photo. 
    /// This BlobName is unique and is used to identify the image in Azure Blob Storage.
    /// </summary>
    public string BlobName { get; set; }

    /// <summary>
    /// Gets or sets the name of the background photo.
    /// This is set by the end user.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the image type of the background photo.
    /// This is either jpg or png.
    /// </summary>
    public ImageType ImageType { get; set; }
}
