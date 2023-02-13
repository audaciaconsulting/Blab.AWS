using System.Globalization;
using Blab.Services.Models.Validation;
using Blab.DataAccess;
using Blab.Services.Extensions.FluentValidationExtensions;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Audacia.Azure.BlobStorage.AddBlob;
using Audacia.Azure.BlobStorage.AddBlob.Commands;
using Audacia.Azure.Common.ReturnOptions.ImageOption;
using Blab.Domain.Models.Photo;
using Audacia.Azure.BlobStorage.UpdateBlob.Commands;
using Audacia.Azure.BlobStorage.UpdateBlob;
using Blab.Domain.Entities.Security;

namespace Blab.Services.Services.Users.UpdateUser;
public class UpdateUserService : IUpdateUserService
{
    private readonly BlabDbContext _context;

    private readonly IAddAzureBlobStorageService _addAzureBlobStorageService;

    private readonly IUpdateAzureBlobStorageService _updateAzureBlobStorageService;

    public UpdateUserService(BlabDbContext context, IAddAzureBlobStorageService addAzureBlobStorageService, IUpdateAzureBlobStorageService updateAzureBlobStorageService)
    {
        _context = context;
        _addAzureBlobStorageService = addAzureBlobStorageService;
        _updateAzureBlobStorageService = updateAzureBlobStorageService;
    }

    public async Task<CommandResult<UpdatedUserDto>> HandleAsync(UpdateUserCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await ValidateCommandAsync(command, cancellationToken);

        if (validationResult.IsValid)
        {
            var user = await _context.Users.Include(user => user.ProfilePhoto)
                                           .Include(user => user.BackgroundPhoto)    
                                           .SingleOrDefaultAsync(x => x.Id == command.UserId, cancellationToken);
            if (user is null)
            {
                return CommandResult.Failure<UpdatedUserDto>(new PropertyErrorMessage("userId",
                string.Format(CultureInfo.InvariantCulture, "User with id: {0} does not exist", command.UserId)));
            }

            user.UserName = command.Handle;
            user.NormalizedUserName = command.Handle.ToUpper(CultureInfo.InvariantCulture);
            user.DisplayName = command.DisplayName;
            user.Bio = command.Bio;

            if (command.ProfilePhoto != null)
            {
                var photoValidationResult = await ValidatePhotoAsync(command.ProfilePhoto, cancellationToken);
                if (!photoValidationResult.IsValid)
                {
                    return CommandResult.Failure<UpdatedUserDto>(photoValidationResult.Errors.ErrorMessages().ToList());
                }

               await AddOrUpdateProfilePhotoAsync(command, user, cancellationToken);
            }

            if (command.BackgroundPhoto != null)
            {
                var photoValidationResult = await ValidatePhotoAsync(command.BackgroundPhoto, cancellationToken);
                if (!photoValidationResult.IsValid)
                {
                    return CommandResult.Failure<UpdatedUserDto>(photoValidationResult.Errors.ErrorMessages().ToList());
                }

                await AddOrUpdateBackgroundPhotoAsync(command, user, cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken: cancellationToken);
            var updatedUser = UpdatedUserDto.FromUser.Compile().Invoke(user, "user-profile-profile-photos", "user-profile-background-photos");
            return new CommandResult<UpdatedUserDto>(updatedUser);
        }

        return CommandResult.Failure<UpdatedUserDto>(validationResult.Errors.ErrorMessages().ToList());
    }

    // Validates edited user details.
    private async Task<ValidationResult> ValidateCommandAsync(UpdateUserCommand command,
        CancellationToken cancellationToken)
    {
        var validator = new UpdateUserValidator(_context.Users, command.UserId);
        return await validator.ValidateAsync(command, cancellationToken);
    }

    // Validates the new profile photo.
    private async Task<ValidationResult> ValidatePhotoAsync(UpdatePhotoRequest photo,
        CancellationToken cancellationToken)
    {
        var validator = new UpdatePhotoValidator();
        return await validator.ValidateAsync(photo, cancellationToken);
    }

    private async Task<ProfilePhoto> AddNewProfilePhotoToAzureAsync(UpdateUserCommand updateUserCommand, CancellationToken cancellationToken)
    {
        var fileExtension = updateUserCommand.ProfilePhoto.ImageType;
        var imageTypeValue = fileExtension.ToLower() == "jpeg"
            ? ImageType.Jpg
            : fileExtension.ToLower() == "png"
                ? ImageType.Png
                : ImageType.None;
        var uniqueBlobName = $"{Guid.NewGuid().ToString()}.{fileExtension}";
        var command = new AddBlobBaseSixtyFourCommand("user-profile-profile-photos", uniqueBlobName, updateUserCommand.ProfilePhoto.Base64Photo);
        if (await _addAzureBlobStorageService.ExecuteAsync(command))
        {
            return new ProfilePhoto()
            {
                UserId = updateUserCommand.LoggedInUserId,
                BlobName = uniqueBlobName,
                Name = updateUserCommand.ProfilePhoto.Name,
                ImageType = imageTypeValue
            };
        }

        return new ProfilePhoto();
    }

    private async Task<BackgroundPhoto> AddNewBackgroundPhotoToAzureAsync(UpdateUserCommand updateUserCommand, CancellationToken cancellationToken)
    {
        var fileExtension = updateUserCommand.BackgroundPhoto.ImageType;
        var imageTypeValue = fileExtension.ToLower() == "jpeg"
            ? ImageType.Jpg
            : fileExtension.ToLower() == "png"
                ? ImageType.Png
                : ImageType.None;
        var uniqueBlobName = $"{Guid.NewGuid().ToString()}.{fileExtension}";
        var command = new AddBlobBaseSixtyFourCommand("user-profile-background-photos", uniqueBlobName, updateUserCommand.BackgroundPhoto.Base64Photo);
        if (await _addAzureBlobStorageService.ExecuteAsync(command))
        {
            return new BackgroundPhoto()
            {
                UserId = updateUserCommand.LoggedInUserId,
                BlobName = uniqueBlobName,
                Name = updateUserCommand.BackgroundPhoto.Name,
                ImageType = imageTypeValue
            };
        }

        return new BackgroundPhoto();
    }

    private async Task<ProfilePhoto> UpdateProfilePhotoInAzureAsync(UpdateUserCommand updateUserCommand, string profilePhotoBlobName, CancellationToken cancellation)
    {
        var fileExtension = updateUserCommand.ProfilePhoto.ImageType;
        var imageTypeValue = fileExtension.ToLower() == "jpeg"
            ? ImageType.Jpg
            : fileExtension.ToLower() == "png"
                ? ImageType.Png
                : ImageType.None;
        var profilePhotoBytes = Convert.FromBase64String(updateUserCommand.ProfilePhoto.Base64Photo);
        var command = new UpdateBlobStorageBytesCommand("user-profile-profile-photos", profilePhotoBlobName, profilePhotoBytes);

        if (await _updateAzureBlobStorageService.ExecuteAsync(command))
        {
            return new ProfilePhoto()
            {
                UserId = updateUserCommand.LoggedInUserId,
                BlobName = profilePhotoBlobName,
                Name = updateUserCommand.ProfilePhoto.Name,
                ImageType = imageTypeValue
            };
        }

        return new ProfilePhoto();
    }

    private async Task<BackgroundPhoto> UpdateBackgroundPhotoInAzureAsync(UpdateUserCommand updateUserCommand, string backgroundPhotoBlobName, CancellationToken cancellation)
    {
        var fileExtension = updateUserCommand.BackgroundPhoto.ImageType;
        var imageTypeValue = fileExtension.ToLower() == "jpeg"
            ? ImageType.Jpg
            : fileExtension.ToLower() == "png"
                ? ImageType.Png
                : ImageType.None;
        var backgroundPhotoBytes = Convert.FromBase64String(updateUserCommand.BackgroundPhoto.Base64Photo);
        var command = new UpdateBlobStorageBytesCommand("user-profile-background-photos", backgroundPhotoBlobName, backgroundPhotoBytes);

        if (await _updateAzureBlobStorageService.ExecuteAsync(command))
        {
            return new BackgroundPhoto()
            {
                UserId = updateUserCommand.LoggedInUserId,
                BlobName = backgroundPhotoBlobName,
                Name = updateUserCommand.BackgroundPhoto.Name,
                ImageType = imageTypeValue
            };
        }

        return new BackgroundPhoto();
    }

    private async Task<ApplicationUser> AddOrUpdateBackgroundPhotoAsync(UpdateUserCommand command, ApplicationUser user, CancellationToken cancellationToken)
    {
        // If the user has not set a background photo before, add new background photo
        if (user.BackgroundPhoto.Id == 0)
        {
            var newBackgroundPhoto = await AddNewBackgroundPhotoToAzureAsync(command, cancellationToken);
            user.BackgroundPhoto = newBackgroundPhoto;
            user.BackgroundPhoto.BlobName = newBackgroundPhoto.BlobName;
            user.BackgroundPhoto.ImageType = newBackgroundPhoto.ImageType;
            user.BackgroundPhoto.Name = newBackgroundPhoto.Name;

            return user;
        }

        //else update the exisiting photo in blob storage, associated with the logged in user.
        var updatedBackgroundPhoto = await UpdateBackgroundPhotoInAzureAsync(command, user.BackgroundPhoto.BlobName, cancellationToken);
        user.BackgroundPhoto.BlobName = updatedBackgroundPhoto.BlobName;
        user.BackgroundPhoto.ImageType = updatedBackgroundPhoto.ImageType;
        user.BackgroundPhoto.Name = updatedBackgroundPhoto.Name;

        return user;
    }

    private async Task<ApplicationUser> AddOrUpdateProfilePhotoAsync(UpdateUserCommand command, ApplicationUser user, CancellationToken cancellationToken)
    {
        // if the user has not set a profile picture before, add new profile picture
        if (user.ProfilePhoto.Id == 0)
        {
            var newProfilePhoto = await AddNewProfilePhotoToAzureAsync(command, cancellationToken);
            user.ProfilePhoto = newProfilePhoto;
            user.ProfilePhoto.BlobName = newProfilePhoto.BlobName;
            user.ProfilePhoto.ImageType = newProfilePhoto.ImageType;
            user.ProfilePhoto.Name = newProfilePhoto.Name;

            return user;
        }

        // else, update the current picture that they have
        var updatedProfilePhoto = await UpdateProfilePhotoInAzureAsync(command, user.ProfilePhoto.BlobName, cancellationToken);
        user.ProfilePhoto.BlobName = updatedProfilePhoto.BlobName;
        user.ProfilePhoto.ImageType = updatedProfilePhoto.ImageType;
        user.ProfilePhoto.Name = updatedProfilePhoto.Name;

        return user;
    }
}
