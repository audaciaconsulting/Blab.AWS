using System.Linq;
using System.Linq.Expressions;
using System.Security.Policy;
using Blab.Api.Requests.Posts;
using Blab.Domain.Entities.Security;
using Blab.Domain.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Blab.Api.Requests.Users;

/// <summary>
/// DTO for user profile.
/// </summary>
public record UserProfileDto
{
    /// <summary>
    /// Gets UserId.
    /// </summary>
    public int UserId { get; init; }

    /// <summary>
    /// Gets the user handle/username.
    /// </summary>
    public string Handle { get; init; } = string.Empty;

    /// <summary>
    /// Gets the user display name.
    /// </summary>
    public string DisplayName { get; init; } = string.Empty;

    /// <summary>
    /// Gets the user bio.
    /// </summary>
    public string Bio { get; init; } = string.Empty;

    /// <summary>
    /// Gets the users blabs.
    /// </summary>
    public IEnumerable<PostDto> Blabs { get; init; } = Enumerable.Empty<PostDto>();

    /// <summary>
    /// Gets a value indicating whether the logged in
    /// user is following the requested user, null if it is the logged in user.
    /// </summary>
    public bool? IsFollowing { get; init; } = false;

    /// <summary>
    /// Gets the url for the profile picture.
    /// </summary>
    public PhotoDto? ProfilePhoto { get; init; }

    /// <summary>
    /// Gets the url for the background photo.
    /// </summary>
    public PhotoDto? BackgroundPhoto { get; init; }

    /// <summary>
    /// Gets UserProfileDto from User.
    /// </summary>
    public static Expression<Func<ApplicationUser, int, string, string,  UserProfileDto>> FromUser { get; } = (user, loggedInUserId, profileContainerUrl, backgroundContianerUrl) => new UserProfileDto()
    {
        UserId = user.Id,
        Handle = user.UserName,
        DisplayName = user.DisplayName,
        Bio = user.Bio,
        Blabs = user.Posts.OrderByDescending(blab => blab.CreatedDate).AsQueryable().Include(post => post.User).ThenInclude(user => user.ProfilePhoto).Select(post => PostDto.FromPost.Compile().Invoke(post, loggedInUserId, profileContainerUrl)),
        IsFollowing = user.Id == loggedInUserId 
        ? null
        : user.Followee.Any(follow => follow.FollowerId == loggedInUserId && follow.FolloweeId == user.Id),
        ProfilePhoto = PhotoDto.FromProfilePhoto.Compile().Invoke(user.ProfilePhoto, profileContainerUrl),
        BackgroundPhoto = PhotoDto.FromBackgroundPhoto.Compile().Invoke(user.BackgroundPhoto, backgroundContianerUrl)
    };
}
