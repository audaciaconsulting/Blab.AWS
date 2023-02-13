using Audacia.Commands;

namespace Blab.Services.Services.Users.UpdateUser;
public class UpdateUserCommand : ICommand
{
    public int LoggedInUserId { get; }

    public int UserId { get; }

    public string Handle { get; }

    public string DisplayName { get; }

    public string Bio { get; }

    public UpdatePhotoRequest? ProfilePhoto { get; }

    public UpdatePhotoRequest? BackgroundPhoto { get; }  

    public UpdateUserCommand(int loggedInUserId, int userId, string handle, string displayName, string bio, UpdatePhotoRequest profilePhoto, UpdatePhotoRequest backgroundPhoto)
    {
        LoggedInUserId = loggedInUserId;
        UserId = userId;
        Handle = handle;
        DisplayName = displayName;
        Bio = bio;
        ProfilePhoto = profilePhoto;
        BackgroundPhoto = backgroundPhoto;
    }
}
