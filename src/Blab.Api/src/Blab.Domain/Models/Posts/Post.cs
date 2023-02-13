using System.Collections.Generic;
using Blab.Domain.Entities.Security;
using Blab.Domain.Models.Reactions;

using Blab.Domain.Models.Comments;

namespace Blab.Domain.Models.Posts;

/// <summary>
/// This is the class for adding the "Blab" but is named post to avoid confusion.
/// This Post Class defines the properties on the class. 
/// A Blab will have one User.
/// A user has many Blabs.  
/// </summary>
public class Post : IPost
{
    /// <summary>
    /// Gets or sets the Id of the post (Blab).
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the Content of the post (Blab).
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date the post (Blab) was created. 
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Gets or sets the user who is posting the BLab. 
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets the navigational property for the user
    /// to link the Blab to the userid. 
    /// </summary>
    public ApplicationUser User { get; set; }

    /// <summary>
    /// Gets or sets the reactions on a post.
    /// </summary>
    public ICollection<Reaction> Reactions { get; set; } = new HashSet<Reaction>();
    /// <summary>
    /// Gets or sets the comments on a post.
    /// </summary>
    public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
}
