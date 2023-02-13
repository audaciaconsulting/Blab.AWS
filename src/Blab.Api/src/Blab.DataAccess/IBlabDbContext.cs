using Blab.Domain.Entities.Models;
using Blab.Domain.Entities.Security;
using Blab.Domain.Models;
using Blab.Domain.Models.Comments;
using Blab.Domain.Models.Photo;
using Blab.Domain.Models.Posts;
using Blab.Domain.Models.Reactions;
using Microsoft.EntityFrameworkCore;

namespace Blab.DataAccess;

public interface IBlabDbContext
{
    DbSet<Post> Posts { get; set; }
    
    DbSet<Chat> Chats { get; set; }
    
    DbSet<ApplicationUser> Users { get; set; }
    
    DbSet<UserChats> UserChats { get; set; }
    
    DbSet<Message> Messages { get; set; }

    DbSet<Reaction> Reactions { get; set; } 

    DbSet<Follow> Follows { get; set; }

    DbSet<Comment> Comments { get; set; }

    DbSet<ProfilePhoto> ProfilePhotos { get; set; }

    DbSet<BackgroundPhoto> BackgroundPhotos { get; set; }
}
