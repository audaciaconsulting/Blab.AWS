using Blab.Domain.Models;
using Blab.Domain.Models.Reactions;

namespace Blab.Api.Requests.Reactions;

/// <summary>
/// Request object for adding a reaction to a blab.
/// </summary>
public class AddReactionRequest
{
    /// <summary>
    /// Gets or sets blabId.
    /// </summary>
    public int PostId { get; set; }

    /// <summary>
    /// Gets or sets UserId.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets reaction on blab/post.
    /// </summary>
    public ReactionType ReactionType { get; set; }
}
