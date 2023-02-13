using Blab.Domain.Models.Photo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blab.DataAccess.EntityConfigurations;
public class ProfilePhotoEntityConfiguration : IEntityTypeConfiguration<ProfilePhoto>
{
    public void Configure(EntityTypeBuilder<ProfilePhoto> builder)
    {
        builder.ToTable("ProfilePhotos");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.BlobName).IsRequired();
        builder.Property(p => p.Name).IsRequired();
        builder.Property(p => p.ImageType).IsRequired();

        builder.HasOne(p => p.User)
            .WithOne(u => u.ProfilePhoto);
    }
}
