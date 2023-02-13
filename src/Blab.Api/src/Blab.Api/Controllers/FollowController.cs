using Blab.Api.Requests.Follows;
using Blab.DataAccess;
using Blab.Domain.Entities.Security;
using Blab.Services.Models.Validation;
using Blab.Services.Services.Follows.AddFollow;
using Blab.Services.Services.Follows.DeleteFollow;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blab.Api.Controllers;

/// <summary>
/// Api controller for requests relating to the follow function.
/// </summary>
[ApiController]
[Route("api/users/follow")]
public class FollowController : ControllerBase
{
    private readonly BlabDbContext _context;

    private readonly UserManager<ApplicationUser> _userManager;

    private readonly IAddFollowService _addFollowService;

    private readonly IDeleteFollowService _deleteFollowService;

    /// <summary>
    /// Initializes new instance of <see cref="FollowController"/> class.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="addFollowService"></param>
    /// <param name="deleteFollowService"></param>
    /// <param name="userManager"></param>
    public FollowController(BlabDbContext context, UserManager<ApplicationUser> userManager,
        IAddFollowService addFollowService, IDeleteFollowService deleteFollowService)
    {
        _context = context;
        _userManager = userManager;
        _addFollowService = addFollowService;
        _deleteFollowService = deleteFollowService;
    }

    /// <summary>
    /// Endpoint to follow another user.
    /// </summary>
    /// <param name="request"></param>
    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(CommandResult), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(CommandResult), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Add(FollowRequest request)
    {
        var checkUserIdValid = int.TryParse(_userManager.GetUserId(User), out var loggedInUserId);
        var command = new AddFollowCommand(request.FollowerId, request.FolloweeId, loggedInUserId);

        var result = await _addFollowService.HandleAsync(command);

        if (result.IsSuccess)
        {
            return CreatedAtAction("Add", result);
        }

        return BadRequest(result);
    }

    /// <summary>
    /// Endpoint to unfollow another user.
    /// </summary>
    /// <param name="followeeId"></param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete]
    [Route("{followeeId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(CommandResult), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(int followeeId)
    {
        var checkUserIdValid = int.TryParse(_userManager.GetUserId(User), out var loggedInUserId);
        var command = new DeleteFollowCommand(loggedInUserId, followeeId);

        var result = await _deleteFollowService.HandleAsync(command);

        if (result.IsSuccess)
        {
            return NoContent();
        }

        return BadRequest(result);
    }
}