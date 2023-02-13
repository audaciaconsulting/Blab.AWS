using System.Linq.Expressions;
using Blab.Domain.Models.Comments;

namespace Blab.Services.Services.Comments.SearchComments;

/// <summary>
/// Dto for comment returned for single blab view. 
/// </summary>
public class CommentDto
{
    /// <summary>
    /// Gets or sets the display name of the user who made the comment.
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the handle of the user who made the comment.
    /// </summary>
    public string Handle { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the content of the comment.
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the created date time of the comment.
    /// </summary>
    public DateTime Created { get; set; }

    /// <summary>
    /// Gets or sets the blobName of the profile photo.
    /// </summary>
    public string ProfilePhotoBlob { get; set; } = string.Empty;

    public static Expression<Func<Comment, string, CommentDto>> FromComment { get; } = (comment, ProfileContainerUrl) => new CommentDto()
    {
        DisplayName = comment.User.DisplayName,
        Handle = comment.User.UserName,
        Content = comment.Content,
        Created = comment.Created,
        ProfilePhotoBlob = ProfileContainerUrl + comment.User.ProfilePhoto.BlobName
    };
}
