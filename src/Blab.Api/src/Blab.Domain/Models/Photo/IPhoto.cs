using Audacia.Azure.Common.ReturnOptions.ImageOption;
using Blab.Domain.Entities.Security;

namespace Blab.Domain.Models.Photo;
public interface IPhoto
{
    /// <summary>
    /// Gets or sets the Id of the photo.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the user Id of the user the photo belongs to.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets the navigation property of the user.
    /// </summary>
    public ApplicationUser User { get; set; }

    /// <summary>
    /// Gets or sets the Blob Name of the photo in Azure Blob storage.
    /// </summary>
    public string BlobName { get; set; }

    /// <summary>
    /// Gets or sets the name of the image uploaded by the user.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the image type of the Photo.
    /// </summary>
    public ImageType ImageType { get; set; }
}