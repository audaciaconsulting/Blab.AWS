using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blab.Domain.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blab.DataAccess.EntityConfigurations;

/// <summary>
/// This configures the entity configuration of Chats. Users are required and the relationship between them is configured in the UserChatsEntityConfiguration.
/// <see cref="BlabDbContext"/>
/// <seealso cref="UserChatsEntityConfiguration"/>
/// </summary>
public class ChatEntityConfiguration : IEntityTypeConfiguration<Chat>
{
    public void Configure(EntityTypeBuilder<Chat> builder)
    {
        builder.ToTable("Chats");

        builder.HasKey(chat => chat.Id);

        builder.HasMany(chat => chat.Users)
            .WithMany(user => user.Chats)
            .UsingEntity<UserChats>(
                joinEntity =>
                    joinEntity.HasOne(userChat => userChat.User)
                        .WithMany(user => user.UserChats)
                        .HasForeignKey(userChat => userChat.UserId),
                joinEntity =>
                    joinEntity.HasOne(userChat => userChat.Chat)
                        .WithMany(chat => chat.UserChats)
                        .HasForeignKey(userChat => userChat.ChatId),
                joinEntity => joinEntity.HasKey(userChat => new { userChat.ChatId, userChat.UserId }));
    }
}
