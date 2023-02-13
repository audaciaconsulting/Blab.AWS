using FluentValidation;
using Blab.Api.Requests.Posts;
using Blab.Domain.Entities.Security;
using Blab.DataAccess;
using Microsoft.AspNetCore.Identity;
using Blab.Api.Validators.Posts;
using Blab.Domain.Models.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blab.Api.Validators.Posts;

/// <summary>
/// Validation for adding Blab posts
/// </summary>
public class AddPostValidator : AbstractValidator<AddPostRequest>
{
    private readonly BlabDbContext _context;
    private readonly int _loggedInUserId;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddPostValidator"/> class.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="loggedInUserId"></param>
    public AddPostValidator(BlabDbContext context, int loggedInUserId)
    {
        _context = context;
        _loggedInUserId = loggedInUserId;

        RuleFor(addPostRequest => addPostRequest.Content)
            .NotEmpty()
            .WithMessage("Blab cannot be empty.")
            .MaximumLength(256)
            .WithMessage("Your Blab cannot exceed 256 characters.");

        RuleFor(addPostRequest => addPostRequest.UserId)
            .Must(ExistingUser)
            .WithMessage("User does not exist")
            .Equal(loggedInUserId)
            .WithMessage("You are unable to add the Blab for another user");
    }

    /// <summary>
    /// a new bool for checking if user posting already exists. 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public bool ExistingUser(int userId)
    {
        var isUserValid = _context.Users.SingleOrDefault(user => user.Id == userId);

        return isUserValid != null;
    }
}