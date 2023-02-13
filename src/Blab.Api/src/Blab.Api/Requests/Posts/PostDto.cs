using System.Linq.Expressions;
using Blab.Domain.Models.Posts;
using Blab.Domain.Models.Reactions;

namespace Blab.Api.Requests.Posts;

/// <summary>
/// Post request model.
/// </summary>
public class PostDto : IPost
{
    /// <summary>
    /// Gets or sets post Id.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets post content.
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets post creation data.
    /// </summary>
    public DateTime DateCreated { get; set; }

    /// <summary>
    ///  Gets or sets userId of person who made post.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets the users handle/username.
    /// </summary>
    public string Handle { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the users display name.
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the current logged in users reaction on the post.
    /// </summary>
    public ReactionType? Reaction { get; set; }

    /// <summary>
    /// Gets or sets the blobName of the profile photo
    /// </summary>
    public string ProfilePhotoBlob { get; set; } = string.Empty;

    /// <summary>
    /// Gets getPostRequest from Post. 
    /// </summary>
    public static Expression<Func<Post, int, string, PostDto>> FromPost { get; } = (post, currentLoggedInUserId, profileContainerUrl) => new PostDto()
    {
        Id = post.Id,
        Content = post.Content,
        DateCreated = post.CreatedDate,
        UserId = post.UserId,
        Handle = post.User.UserName,
        DisplayName = post.User.DisplayName,
        Reaction = post.Reactions.SingleOrDefault(reaction => reaction.UserId == currentLoggedInUserId) != null 
            ? post.Reactions.Single(reaction => reaction.UserId == currentLoggedInUserId).ReactionType
            : null,
        ProfilePhotoBlob = profileContainerUrl + post.User.ProfilePhoto.BlobName
    };
}
