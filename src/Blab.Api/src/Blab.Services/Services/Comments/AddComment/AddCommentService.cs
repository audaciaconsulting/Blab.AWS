using Blab.DataAccess;
using Blab.Domain.Models.Comments;
using Blab.Services.Extensions.FluentValidationExtensions;
using Blab.Services.Models.Validation;
using FluentValidation.Results;

namespace Blab.Services.Services.Comments.AddComment;
public class AddCommentService : IAddCommentService
{
    private readonly BlabDbContext _context;

    public AddCommentService(BlabDbContext context)
    {
        _context = context;
    }

    public async Task<CommandResult<int>> HandleAsync(AddCommentCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await ValidateCommandAsync(command, cancellationToken);
        if (validationResult.IsValid)
        {
            var newComment = new Comment()
            {
                PostId = command.PostId,
                UserId = command.UserId,
                Content = command.Content,
                Created = DateTime.Now
            };
            _context.Comments.Add(newComment);
            await _context.SaveChangesAsync(cancellationToken);

            return new CommandResult<int>(newComment.Id);
        }

        return CommandResult.Failure<int>(validationResult.Errors.ErrorMessages().ToList());
    }

    private async Task<ValidationResult> ValidateCommandAsync(AddCommentCommand command, CancellationToken cancellationToken)
    {
        var validator = new AddCommentValidator(_context, cancellationToken);
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        return validationResult;
    }
}
