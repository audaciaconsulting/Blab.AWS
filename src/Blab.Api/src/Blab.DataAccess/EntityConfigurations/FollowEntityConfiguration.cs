using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blab.Domain.Entities.Security;
using Blab.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Blab.DataAccess.EntityConfigurations;

public class FollowEntityConfiguration : IEntityTypeConfiguration<Follow>
{
    public void Configure(EntityTypeBuilder<Follow> builder)
    {
        builder.ToTable("Follow");

        builder.HasKey(follow => new { follow.FollowerId, follow.FolloweeId });

        builder.HasOne(f => f.Followee)
            .WithMany(u => u.Follower)
            .HasForeignKey(f => f.FollowerId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(f => f.Follower)
            .WithMany(u => u.Followee)
            .HasForeignKey(f => f.FolloweeId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}