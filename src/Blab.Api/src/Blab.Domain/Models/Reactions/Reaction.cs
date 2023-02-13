using Blab.Domain.Entities.Security;
using Blab.Domain.Models.Posts;

namespace Blab.Domain.Models.Reactions;

/// <summary>
/// Reaction class for a user reacting to a post.
/// </summary>
public class Reaction
{
    /// <summary>
    /// Gets or sets the id of the reaction.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the id of the post being reacted to, foreign key.
    /// </summary>
    public int PostId { get; set; }

    /// <summary>
    /// Post navigation property.
    /// </summary>
    public Post Post { get; set; }

    /// <summary>
    /// Gets or sets the id of the user who is reacting, foreign key.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// User navigation property
    /// </summary>
    public ApplicationUser User { get; set; }

    /// <summary>
    /// Gets or sets the chosen reaction.
    /// </summary>
    public ReactionType ReactionType { get; set; }

    /// <summary>
    /// Gets or sets the Datetime of the reaction.
    /// </summary>
    public DateTime Created { get; set; }
}
