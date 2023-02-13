using Blab.Api.Requests.Chats;
using Blab.Api.Validators.Messages;
using Blab.DataAccess;
using Blab.Domain.Entities.Models;
using Blab.Domain.Entities.Security;
using Blab.Services.Models.Validation;
using Blab.Services.Services.UserChats.DeleteUserChat;
using Blab.Services.Services.UserChats.GetAllChatsForLoggedInUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blab.Api.Controllers;

/// <summary>
/// Api Controller for requests relating to the Chat Function.
/// </summary>
[ApiController]
[Route("api/chats")]
public class ChatsController : ControllerBase
{
    private readonly BlabDbContext _context;

    private readonly UserManager<ApplicationUser> _userManager;

    private readonly IDeleteUserChatService _deleteUserChatService;

    private readonly IGetAllChatsForLoggedInUserService _getAllChatsForLoggedInUserService;

    /// <summary>
    /// Context is used to refer to data in the SQL database. 
    /// The UserManager is a library that is used to capture information about the User that is logged into the application.
    /// </summary>
    /// <param name="context">Refers to data in the SQL database.</param>
    /// <param name="userManager"></param>
    /// <param name="deleteUserChatService"></param>
    /// <param name="getAllChatsForLoggedInUserService"></param>
    public ChatsController(BlabDbContext context, UserManager<ApplicationUser> userManager,
        IDeleteUserChatService deleteUserChatService, IGetAllChatsForLoggedInUserService getAllChatsForLoggedInUserService)
    {
        _context = context;
        _userManager = userManager;
        _deleteUserChatService = deleteUserChatService;
        _getAllChatsForLoggedInUserService = getAllChatsForLoggedInUserService;
    }

    /// <summary>
    /// This is an endpoint returns the chatId between a loggedInUser and a specified otherUser.
    /// </summary>
    /// <param name="loggedInUserId"></param>
    /// <param name="otherUserId"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("{loggedInUserId}/{otherUserId}")]
    [Authorize]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetChatId(int loggedInUserId, int otherUserId)
    {
        var chat = await _context.Chats.AsNoTracking().SingleOrDefaultAsync(chat => chat.Users.Count(user => user.Id == otherUserId || user.Id == loggedInUserId) == 2);
        if (chat != null)
        {
            return Ok(chat.Id);
        }

        return NotFound("A chat between these users does not exist");
    }

    /// <summary>
    /// Get a list of all the chats that a user is apart of.
    /// The userId of the loggedinuser is not requested but found using the userManager.
    /// </summary>
    /// <returns> A list of chatIds, if request is successful</returns>
    [HttpGet]
    [Route("all")]
    [Authorize]
    [ProducesResponseType(typeof(CommandResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CommandResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllChats()
    {
        //This gets the userId of the loggedInUser through tokens.
        var isLoggedInUserIdAnInt = int.TryParse(_userManager.GetUserId(User), out var loggedInUserId);

        var command = new GetAllChatsForLoggedInUserCommand(loggedInUserId);
        
        var result = await _getAllChatsForLoggedInUserService.HandleAsync(command);

        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return NotFound(result);
    }

    /// <summary>
    /// This is the Endpoint for a Loggedin User to create a chat with another User. 
    /// </summary>
    /// <param name="chat">This param contains a LoggedInUserId and the OtherUserId (the Id of the User they want to add). </param>
    /// <returns>A chat between the two users is created if validation is met,else a BadRequest is returned and the chat is not created.</returns>
    [HttpPost]
    [Route("add")]
    [Authorize]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(List<FluentValidation.Results.ValidationFailure>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateChat(CreateNewChat chat)
    {
        // This gets the userId of the loggedInUser through tokens.
        if (!int.TryParse(_userManager.GetUserId(User), out var loggedInUserId))
        {
            return BadRequest();
        }

        var validator = new AddChatValidator(_context, loggedInUserId);
        var validationResult = await validator.ValidateAsync(chat);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var newChat = new Chat(chat.LoggedInUserId, chat.OtherUserId);

        _context.Chats.Add(newChat);
        await _context.SaveChangesAsync();

        return Ok(newChat.Id);
    }

    /// <summary>
    /// Removes a <see cref="Chat"/> with the Id <paramref name="chatId"/>.
    /// </summary>
    /// <param name="chatId">Id of a <see cref="Chat"/>.</param>
    /// <returns>A <see cref="CommandResult"/> if the delete operation was successful.</returns>
    [HttpDelete]
    [Authorize]
    [Route("{chatId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(CommandResult), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(int chatId)
    {
        if (!int.TryParse(_userManager.GetUserId(User), out var loggedInUserId))
        {
            return BadRequest();
        }

        var command = new DeleteUserChatCommand(chatId, loggedInUserId);

        var result = await _deleteUserChatService.HandleAsync(command);

        if (result.IsSuccess)
        {
            return NoContent();
        }

        return BadRequest(result);
    }
}
