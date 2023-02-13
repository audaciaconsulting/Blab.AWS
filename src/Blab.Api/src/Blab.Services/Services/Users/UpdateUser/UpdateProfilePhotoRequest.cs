using System.Drawing;
using Audacia.Azure.Common.ReturnOptions.ImageOption;

namespace Blab.Services.Services.Users.UpdateUser;

/// <summary>
/// Update profile photo request.
/// </summary>
public record UpdatePhotoRequest
{
    /// <summary>
    /// Gets photo url.
    /// </summary>
    public string Base64Photo { get; init; } = string.Empty;

    /// <summary>
    /// Gets filename of uploaded photo.
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Gets a string with the image type of the Photo.
    /// </summary>
    public string ImageType { get; init; } = string.Empty;

    /// <summary>
    /// Gets the size of the image in bytes.
    /// </summary>
    public int Size { get; init; }
}
