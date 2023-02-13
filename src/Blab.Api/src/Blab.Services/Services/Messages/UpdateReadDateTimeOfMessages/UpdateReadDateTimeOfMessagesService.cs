using Blab.DataAccess;
using Blab.Domain.Entities.Models;
using Blab.Services.Extensions.FluentValidationExtensions;
using Blab.Services.Models.Validation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace Blab.Services.Services.Messages.UpdateReadDateTimeOfMessages;
public class UpdateReadDateTimeOfMessagesService : IUpdateReadDateTimeOfMessagesService
{
    private readonly BlabDbContext _context;

    public UpdateReadDateTimeOfMessagesService(BlabDbContext context)
    {
        _context = context;
    }

    public async Task<CommandResult<IEnumerable<ReadMessagesDto>>> HandleAsync(UpdateReadDateTimeOfMessagesCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await ValidateCommandAsync(command, cancellationToken);

        if (validationResult.IsValid)
        {
            // The unread messages in the chat are ones that arent sent by the logged in user and where their ReadDateTime is null. They are then selected and their ReadDateTime property is set to now.
            var unreadMessages = await _context.Messages.AsQueryable()
                                                        .Where(message => message.ChatId == command.ChatId && message.UserId != command.LoggedInUserId && message.ReadDateTime == null)
                                                        .ToListAsync();
            foreach (var unreadMessage in unreadMessages)
            {
                unreadMessage.ReadDateTime = DateTime.Now;
            }

            await _context.SaveChangesAsync(cancellationToken: cancellationToken);
            return new CommandResult<IEnumerable<ReadMessagesDto>>(unreadMessages.AsQueryable().Select(ReadMessagesDto.FromMessage));
        }

        return CommandResult.Failure<IEnumerable<ReadMessagesDto>>(validationResult.Errors.ErrorMessages().ToList());
    }

    private async Task<ValidationResult> ValidateCommandAsync(UpdateReadDateTimeOfMessagesCommand command, CancellationToken cancellationToken)
    {
        var validator = new UpdateReadDateTimeOfMessagesValidator(_context);
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        return validationResult;
    }

}