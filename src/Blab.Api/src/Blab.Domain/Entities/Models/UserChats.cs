using Blab.Domain.Entities.Security;

namespace Blab.Domain.Entities.Models;

/// <summary>
/// This class is for the join table that is created from the many to many relationship between Chats and Users
/// A Chat must contain many Users and Users can be in many Chats. This class represents the join table frm this relationship
/// This class is to help create the entity relationship and so that this table can be referred to in controllers for easier valdiation.
/// The UserId and the ChatId are the columns in the UserChats table. but the User and Chat are there as naviagtional properties.
/// </summary>
public class UserChats
{
    public int UserId { get; set; }

    public ApplicationUser User { get; set; }

    public int ChatId { get; set; }

    public Chat Chat { get; set; }
}
