using System.Linq.Expressions;
using Blab.Domain.Models.Posts;
using Blab.Domain.Models.Reactions;

namespace Blab.Services.Services.BlabFeed;
public class PostDtoForBlabFeed
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
    /// Gets or sets the blobName of the profile photo.
    /// </summary>
    public string ProfilePhotoBlob { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the reaction the logged in user has made on that post.
    /// </summary>
    public ReactionType? Reaction { get; set; } 

    /// <summary>
    /// Expresssion to get the necessary details for each post on the blab feed.
    /// </summary>
    public static Expression<Func<Post, int, string, PostDtoForBlabFeed>> FromPost { get; } = (post, loggedInUserId, ProfileContainerUrl) => new PostDtoForBlabFeed()
    {
        Id = post.Id,
        Content = post.Content,
        DateCreated = post.CreatedDate,
        UserId = post.UserId,
        Handle = post.User.UserName,
        DisplayName = post.User.DisplayName,
        ProfilePhotoBlob = ProfileContainerUrl + post.User.ProfilePhoto.BlobName,
        Reaction = post.Reactions.Any(reaction => reaction.UserId == loggedInUserId) ? post.Reactions.Single(reaction => reaction.UserId == loggedInUserId).ReactionType : null
    };
}
