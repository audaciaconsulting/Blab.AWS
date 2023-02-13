using System.ComponentModel.DataAnnotations.Schema;
using Blab.Domain.Models.Posts;
using Blab.Domain.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Blab.Domain.Models;
using Blab.Domain.Models.Comments;
using Blab.Domain.Models.Reactions;
using Blab.Domain.Models.Photo;

namespace Blab.Domain.Entities.Security;

public class ApplicationUser : IdentityUser<int>
{
    public string Name { get; set; } = string.Empty;

    public override string NormalizedUserName => NormalizedEmail;

    public bool HasGivenTrackingConsent { get; set; } = false;

    public DateTimeOffset? HasGivenTrackingConsentOn { get; set; }

    public DateTime? Created { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? Modified { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime DateOfBirth { get; set; }

    public string Bio { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public ProfilePhoto ProfilePhoto { get; set; } = new();

    public BackgroundPhoto BackgroundPhoto { get; set; } = new();

    [InverseProperty("User")]
    public virtual ICollection<ApplicationUserRole> Roles { get; set; } = new List<ApplicationUserRole>();
    
    public ICollection<Post> Posts { get; set;  } = new HashSet<Post>();

    public ICollection<Chat> Chats { get; set; } = new HashSet<Chat>();

    public ICollection<UserChats> UserChats { get; set; } = new HashSet<UserChats>();

    public ICollection<Message> Messages { get; set; } = new HashSet<Message>();

    public ICollection<Reaction> Reactions { get; set; } = new HashSet<Reaction>();

    public ICollection<Follow> Follower { get; set; } = new HashSet<Follow>();

    public ICollection<Follow> Followee { get; set; } = new HashSet<Follow>();

    public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
}
