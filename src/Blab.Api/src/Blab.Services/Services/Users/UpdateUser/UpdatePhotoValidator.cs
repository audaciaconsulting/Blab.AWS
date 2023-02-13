using Blab.Domain.Entities.Security;
using Blab.Services.Services.Users.UpdateUser;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Blab.Services.Services.Users.UpdateUser;

/// <summary>
/// Validator for UpdateProfilePhotoRequest class. 
/// </summary>
public class UpdatePhotoValidator : AbstractValidator<UpdatePhotoRequest>
{
    private readonly int _fiveMB = 5000000;

    /// <summary>
    /// Validation rules for UpdateProfilePhotoRequest.
    /// </summary>
    public UpdatePhotoValidator()
    {
        RuleFor(profilePhoto => profilePhoto.Base64Photo)
            .NotEmpty()
            .WithMessage("Image is not valid.");
        RuleFor(profilePhoto => profilePhoto.Size)
            .NotEmpty()
            .Must(size => size > 0 && size < _fiveMB)
            .WithMessage("Image size must be less than 5MB.");
        RuleFor(profilePhoto => profilePhoto.ImageType)
            .NotEmpty()
            .Must(type => type == "jpeg" || type == "png")
            .WithMessage("Image must be of type JPG or PNG.");
    }
}
