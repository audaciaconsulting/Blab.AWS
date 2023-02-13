namespace Blab.Api.Requests.Comments;

/// <summary>
/// Request object for adding a reaction to a blab.
/// </summary>
public class AddCommentRequest
{
    /// <summary>
    /// Gets or sets the user Id of the user making the comment.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets the content of the comment.
    /// </summary>
    public string Content { get; set; } = string.Empty;
}
