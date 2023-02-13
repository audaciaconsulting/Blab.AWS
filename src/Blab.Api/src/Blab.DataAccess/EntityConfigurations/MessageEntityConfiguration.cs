using Blab.Domain.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blab.DataAccess.EntityConfigurations;
public class MessageEntityConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.HasKey(message => message.Id);

        builder.Property(message => message.Content).IsRequired();

        builder.Property(message => message.Sent).IsRequired();

        builder.Property(message => message.ReadDateTime).HasDefaultValue(null);

        builder.HasOne(message => message.User).WithMany(user => user.Messages);

        builder.HasOne(message => message.Chat).WithMany(chat => chat.Messages);
    }
}
