using Blab.Domain.Models.Comments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blab.DataAccess.EntityConfigurations;

public class CommentEntityConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable("Comments");
        
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Content).IsRequired();
        builder.Property(x => x.Created).IsRequired();

        builder.HasOne(x => x.Post)
            .WithMany(y => y.Comments)
            .HasForeignKey(x => x.PostId)
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.HasOne(x => x.User)
            .WithMany(y => y.Comments)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
