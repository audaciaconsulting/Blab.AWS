using Audacia.Azure.Common.ReturnOptions.ImageOption;
using Blab.Domain.Entities.Security;

namespace Blab.Domain.Models.Photo;

/// <summary>
/// This is the Profile Photo class inherits the propertys of the IPhoto interface.
/// </summary>
public class ProfilePhoto : IPhoto
{
    /// <summary>
    /// Gets or sets the id of the profile photo.
    /// This is set in the sql database.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the userId of the user that profile photo belongs to.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets the navigational property of the user.
    /// </summary>
    public ApplicationUser User { get; set; }

    /// <summary>
    /// Gets or sets the BlobName of profile photo. 
    /// This BlobName is unique and is used to identify the image in Azure Blob Storage.
    /// </summary>
    public string BlobName { get; set; }

    /// <summary>
    /// Gets or sets the name of the profile photo.
    /// This is set by the end user.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the image type of the profile photo.
    /// This is either jpg or png.
    /// </summary>
    public ImageType ImageType { get; set; }
}
