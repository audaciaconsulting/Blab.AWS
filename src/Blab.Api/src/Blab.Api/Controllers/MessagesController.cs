using Blab.Api.Requests.Messages;
using Blab.Api.Validators.Messages;
using Blab.DataAccess;
using Blab.Domain.Entities.Models;
using Blab.Domain.Entities.Security;
using Blab.Services.Models.Validation;
using Blab.Services.Services.Messages.UpdateReadDateTimeOfMessages;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blab.Api.Controllers;

/// <summary>
/// This is an Api Controller for requests related to Messages.
/// </summary>
[ApiController]
public class MessagesController : ControllerBase
{
    private readonly BlabDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUpdateReadDateTimeOfMessagesService _updateReadDateTimeOfMessagesService;

    /// <summary>
    /// Context is used to refer to data in the SQL database. 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="userManager"></param>
    /// <param name="updateReadDateTimeOfMessagesService"></param>
    public MessagesController(BlabDbContext context, UserManager<ApplicationUser> userManager, IUpdateReadDateTimeOfMessagesService updateReadDateTimeOfMessagesService)
    {
        _context = context;
        _userManager = userManager;
        _updateReadDateTimeOfMessagesService = updateReadDateTimeOfMessagesService;
    }

    /// <summary>
    /// This gets the messages from a chatId.
    /// </summary>
    /// <param name="chatId"></param>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    [Route("api/chat/{chatId}/messages")]
    [ProducesResponseType(typeof(IQueryable<Message>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMessagesFromAChat(int chatId)
    {
        // Get the id of the loggedInUser, so that only the loggedInUser can only access the messages from the chat that they are apart of.n
        var isLoggedInUserIdTheSame = int.TryParse(_userManager.GetUserId(User), out var id);

        // Check if chat is valid
        if (!await _context.Chats.AsNoTracking().AnyAsync(theChat => theChat.Id == chatId))
        {
            return NotFound("Chat does not exist.");
        }

        // Check if the logged in user is one of the users in the chat.
        if (!await _context.UserChats.AsNoTracking().AnyAsync(userChat => userChat.ChatId == chatId && userChat.UserId == id))
        {
            return BadRequest("You can't access the messages of a chat if the logged in user is not a member of that chat.");
        }

        var messagesFromUser = _context.Messages.AsNoTracking().Where(messages => messages.ChatId == chatId).OrderByDescending(message => message.Sent).Select(MessagesDto.FromMessage);
        return Ok(messagesFromUser);
    }

    /// <summary>
    /// This HttpPost method is for when a User sends a message in a chat. 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="chatId"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize]
    [Route("api/chat/{chatId}/message")]
    [ProducesResponseType(typeof(Message), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddMessage(AddMessageRequest message, int chatId)
    {
        //This adds the chatId to the AddMessageRequest class to that it can be validated.
        message.ChatId = chatId;
        // The validator validates the UserId and the content from the message variable.
        var validator = new AddMessageValidator(_context, chatId);
        var validationResult = await validator.ValidateAsync(message);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var newMessage = new Message
        {
            UserId = message.UserId,
            ChatId = chatId,
            Content = message.Content,
            Sent = DateTime.Now
        };
        _context.Messages.Add(newMessage);
        await _context.SaveChangesAsync();

        return CreatedAtAction("AddMessage", newMessage);
    }

    /// <summary>
    /// This is the endpoint that get called when a user opens a chat, any unread messages get declared as read with the DateTime that the chat was opened.
    /// </summary>
    /// <param name="chatId"></param>
    /// <returns></returns>
    [HttpPut]
    [Authorize]
    [Route("api/chat/{chatId}/messages/read")]
    [ProducesResponseType(typeof(CommandResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CommandResult), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ReadMessages(int chatId)
    {
        var isLoggedInUserIdValid = int.TryParse(_userManager.GetUserId(User), out var loggedInUserId);

        var command = new UpdateReadDateTimeOfMessagesCommand(chatId, loggedInUserId);

        var result = await _updateReadDateTimeOfMessagesService.HandleAsync(command);

        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
}
