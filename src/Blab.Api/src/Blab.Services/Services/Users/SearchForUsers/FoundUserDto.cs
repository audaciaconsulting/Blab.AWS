using System.Linq.Expressions;
using Blab.Domain.Entities.Security;

namespace Blab.Services.Services;

/// <summary>
/// For each user found that matches the requested search term, only a few properties are needed to be returned.
/// This Dto returns the necessary details for each user in the list of users that match the requested search term.
/// </summary>
public class FoundUserDto
{ 
    /// <summary>
    /// The UserId of the user returned by the database.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// The Handle/Username of the user found.
    /// </summary>
    public string Handle { get; set; }

    /// <summary>
    /// The DisplayName of the user founnd.
    /// </summary>
    public string DisplayName { get; set; }

    public static Expression<Func<ApplicationUser, FoundUserDto>> FromUser { get; } = user => new FoundUserDto()
    {
        UserId = user.Id,
        DisplayName = user.DisplayName,
        Handle = user.UserName
    };
}
