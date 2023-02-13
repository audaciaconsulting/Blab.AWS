using Blab.Domain.Models;
using Blab.Domain.Models.Reactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blab.DataAccess.EntityConfigurations;
public class ReactionEntityConfiguration : IEntityTypeConfiguration<Reaction>
{
    public void Configure(EntityTypeBuilder<Reaction> builder)
    {
        builder.ToTable("Reactions");

        builder.HasKey(reaction => reaction.Id);
        builder.Property(reaction => reaction.ReactionType).IsRequired();
        builder.Property(reaction => reaction.Created).IsRequired();

        builder.HasOne(reaction => reaction.Post)
            .WithMany(post => post.Reactions)
            .HasForeignKey(reaction => reaction.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(reaction => reaction.User)
            .WithMany(user => user.Reactions)
            .HasForeignKey(reaction => reaction.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
