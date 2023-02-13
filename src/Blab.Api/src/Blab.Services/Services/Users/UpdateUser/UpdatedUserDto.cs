using Blab.Domain.Entities.Security;
using System.Linq.Expressions;

namespace Blab.Services.Services.Users.UpdateUser;
public record UpdatedUserDto
{
    /// <summary>
    /// Gets or sets the UserId of the user that has been updated.
    /// This is the same as the logged in user's Id.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets the Handle of the user, this might not have been updated.
    /// </summary>
    public string Handle { get; set; }

    /// <summary>
    /// Gets or sets the Display name of the user, this might not have been updated.
    /// </summary>
    public string DisplayName { get; set; }

    /// <summary>
    /// Gets or sets the Bio of the user, this might not have been updated.
    /// </summary>
    public string Bio { get; set; }

    /// <summary>
    /// Gets or sets the Profile Photo Blob location, this might not have been updated.
    /// If the user has never set a profile picture, this property would be null.
    /// </summary>
    public string? ProfilePhotoBlob { get; set; }

    /// <summary>
    /// Gets or sets the Background Photo Blob location, this might not have been updated.
    /// If the user has never set a background picture, this property would be null.
    /// </summary>
    public string? BackgroundPhotoBlob { get; set; }

    public static Expression<Func<ApplicationUser, string, string, UpdatedUserDto>> FromUser { get; } = (user, profileContainerUrl, backgroundContainerUrl) => new UpdatedUserDto
    {
        UserId = user.Id,
        Handle = user.UserName,
        DisplayName = user.DisplayName,
        Bio = user.Bio,
        ProfilePhotoBlob = profileContainerUrl + user.ProfilePhoto.BlobName,
        BackgroundPhotoBlob = backgroundContainerUrl + user.BackgroundPhoto.BlobName
    };
}
