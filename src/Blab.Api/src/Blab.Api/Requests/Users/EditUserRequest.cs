using System.ComponentModel.DataAnnotations;
using Blab.Services.Services.Users.UpdateUser;

namespace Blab.Api.Requests.Users;

/// <summary>
/// Edit user request.
/// </summary>
public record EditUserRequest
{
    /// <summary>
    /// Gets users UserID.
    /// </summary>
    public int UserId { get; init; }

    /// <summary>
    /// Gets users Handle.
    /// </summary>
    public string Handle { get; init; } = string.Empty;

    /// <summary>
    /// Gets users display name.
    /// </summary>
    public string DisplayName { get; init; } = string.Empty;

    /// <summary>
    /// Gets the users bio.
    /// </summary>
    public string Bio { get; init; } = string.Empty;

    /// <summary>
    /// Gets the file path to the new profile photo.
    /// </summary>
    public UpdatePhotoRequest? ProfilePhoto { get; init; }

    /// <summary>
    /// Gets the file path to the new background photo.
    /// </summary>
    public UpdatePhotoRequest? BackgroundPhoto { get; init; }
}
