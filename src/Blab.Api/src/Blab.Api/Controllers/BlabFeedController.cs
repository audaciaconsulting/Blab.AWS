using Audacia.Core;
using Blab.DataAccess;
using Blab.Domain.Entities.Security;
using Blab.Services.Models.Validation;
using Blab.Services.Services.BlabFeed;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blab.Api.Controllers;

/// <summary>
/// This is a controller for a user's Blab Feed.
/// </summary>
[ApiController]
[Route("api/feed")]
public class BlabFeedController : ControllerBase
{
    private readonly BlabDbContext _context;

    private readonly UserManager<ApplicationUser> _userManager;

    private readonly IGetBlabFeedService _blabFeedService;

    /// <summary>
    /// Constructor for the Blabfeed controller.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="blabFeedService"></param>
    /// <param name="userManager"></param>
    public BlabFeedController(BlabDbContext context, IGetBlabFeedService blabFeedService, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _blabFeedService = blabFeedService;
        _userManager = userManager;
    }

    /// <summary>
    /// Gets the blab feed of the logged in user, based on the posts of the other users that they follow.  
    /// </summary>
    /// <param name="pageSize"></param>
    /// <param name="pageNumber"></param>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(CommandResult<Page<PostDtoForBlabFeed>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CommandResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBlabFeed([FromQuery] int pageSize, [FromQuery] int pageNumber)
    {
        //This gets the userId of the loggedInUser through tokens.
        var isLoggedInUserIdAnInt = int.TryParse(_userManager.GetUserId(User), out var id);
        if (!isLoggedInUserIdAnInt)
        {
            return NotFound("User id not found");
        }

        var command = new GetBlabFeedCommand(id, pageSize, pageNumber);

        var result = await _blabFeedService.HandleAsync(command);

        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
}