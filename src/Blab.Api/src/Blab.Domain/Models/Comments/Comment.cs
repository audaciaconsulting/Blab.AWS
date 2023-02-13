using Blab.Domain.Entities.Security;
using Blab.Domain.Models.Posts;

namespace Blab.Domain.Models.Comments;

/// <summary>
/// Comment class for a user adding a comment to a post. 
/// </summary>
public class Comment
{
    /// <summary>
    /// Gets or sets the id of the comment.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the content of the comment.
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the created datetime of the comment.
    /// </summary>
    public DateTime Created { get; set; }

    /// <summary>
    /// Gets or sets the post id of the post being reacted to, foreign key.
    /// </summary>
    public int PostId { get; set; }

    /// <summary>
    /// Gets or sets Post navigation property.
    /// </summary>
    public Post Post { get; set; }

    /// <summary>
    /// Gets or sets the user id of the user making the comment, foreign key.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets User navigation property.
    /// </summary>
    public ApplicationUser User { get; set; }
}
