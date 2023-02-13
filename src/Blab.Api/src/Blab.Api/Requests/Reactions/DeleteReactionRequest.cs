namespace Blab.Api.Requests.Reactions;

/// <summary>
/// Request object for removing a reaction from a blab.
/// </summary>
public class DeleteReactionRequest
{
    /// <summary>
    /// Gets or sets blabId.
    /// </summary>
    public int PostId { get; set; }

    /// <summary>
    /// Gets or sets UserId.
    /// </summary>
    public int UserId { get; set; }
}
