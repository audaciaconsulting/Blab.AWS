namespace Blab.Api.Requests.Follows;

/// <summary>
/// Request to follow a user.
/// </summary>
public class FollowRequest
{
    /// <summary>
    /// Gets or sets the id of the user requesting to follow someone.
    /// </summary>
    public int FollowerId { get; set; }

    /// <summary>
    /// Gets or sets the id of the user to be followed.
    /// </summary>
    public int FolloweeId { get; set; }
}
