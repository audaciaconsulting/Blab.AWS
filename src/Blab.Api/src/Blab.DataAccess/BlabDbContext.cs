using Blab.DataAccess.EntityConfigurations;
using Blab.DataAccess.Extensions;
using Blab.Domain.Entities.Models;
using Blab.Domain.Entities.Security;
using Blab.Domain.Models;
using Blab.Domain.Models.Comments;
using Blab.Domain.Models.Photo;
using Blab.Domain.Models.Posts;
using Blab.Domain.Models.Reactions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blab.DataAccess;

public class BlabDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>, IBlabDbContext
{
    public DbSet<Post> Posts { get; set; }

    public DbSet<Chat> Chats { get; set; }

    public DbSet<UserChats> UserChats { get; set; }

    public DbSet<Message> Messages { get; set; }

    public DbSet<Reaction> Reactions { get; set; }

    public DbSet<Comment> Comments { get; set; }

    public DbSet<Follow> Follows { get; set; }

    public DbSet<ProfilePhoto> ProfilePhotos { get; set; }

    public DbSet<BackgroundPhoto> BackgroundPhotos { get; set; }

    public BlabDbContext(DbContextOptions<BlabDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Configure(new BlabEntityConfiguration());
        modelBuilder.Configure(new ChatEntityConfiguration());
        modelBuilder.Configure(new MessageEntityConfiguration());
        modelBuilder.Configure(new ReactionEntityConfiguration());
        modelBuilder.Configure(new ApplicationUserEntityConfiguration());
        modelBuilder.Configure(new CommentEntityConfiguration());
        modelBuilder.Configure(new FollowEntityConfiguration());
        modelBuilder.Configure(new ProfilePhotoEntityConfiguration());
        modelBuilder.Configure(new BackgroundPhotoEntityConfiguration());
    }
}
