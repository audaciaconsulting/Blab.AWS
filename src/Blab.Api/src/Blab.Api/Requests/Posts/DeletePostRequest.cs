namespace Blab.Api.Requests.Posts;

/// <summary>
/// Request body for deleting a Blab. 
/// </summary>
public class DeletePostRequest
{
    /// <summary>
    /// Gets or sets the UserId of the User that is logged in and is the User originally created the Blab.
    /// </summary>
    public int LoggedInUserId { get; set; }

    /// <summary>
    /// Gets or sets the Id of the Blab to be deleted. 
    /// /// </summary>
    public int? PostId { get; set; }
}