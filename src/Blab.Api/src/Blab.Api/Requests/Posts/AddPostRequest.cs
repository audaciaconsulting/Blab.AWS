using Blab.Domain.Models.Posts;

namespace Blab.Api.Requests.Posts;

/// <summary>
/// actions relating to adding a new Blab.
/// </summary>
public class AddPostRequest : IPost
{
    /// <summary>
    /// Gets or Sets - adding the post content.
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Gets or Sets - adding the user id of the logged in user.    
    /// </summary>
    public int UserId { get; set; }
}
