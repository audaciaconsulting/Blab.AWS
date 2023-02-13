using System.Configuration;
using Audacia.CodeAnalysis.Analyzers.Helpers.ParameterCount;
using Audacia.Core;
using Blab.Api.Configuration.Options;
using Blab.Api.Requests.Users;
using Blab.DataAccess;
using Blab.Domain.Entities.Security;
using Blab.Services.Models.Validation;
using Blab.Services.Services;
using Blab.Services.Services.BlabFeed;
using Blab.Services.Services.Users.SearchForUsers;
using Blab.Services.Services.Users.UpdateUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Blab.Api.Controllers;

/// <summary>
/// Api users controller.
/// </summary>
[ApiController]
[Route("api/users")]

public class UsersController : ControllerBase
{
    private readonly BlabDbContext _context;
    private readonly IUpdateUserService _updateUserService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ISearchForUsersService _searchForUsersService;
    private readonly IGetProfileBlabFeedService _profileBlabFeedService;
    private readonly IOptions<BlobStorageConfig> _blobStorageConfig;

    /// <summary>
    /// Constructor for user controller. 
    /// </summary>
    [MaxParameterCount(6)]
    public UsersController(
        BlabDbContext context,
        ISearchForUsersService searchForUsersService, 
        IUpdateUserService updateUserService,
        UserManager<ApplicationUser> userManager,
        IGetProfileBlabFeedService profileBlabFeedService,
        IOptions<BlobStorageConfig> blobStorageConfig)
    {
        _context = context;
        _searchForUsersService = searchForUsersService;
        _updateUserService = updateUserService;
        _userManager = userManager;
        _profileBlabFeedService = profileBlabFeedService;
        _blobStorageConfig = blobStorageConfig;
    }

    /// <summary>
    /// Get user profile DTO.
    /// </summary>
    /// <param name="id"></param>
    [Authorize]
    [HttpGet]
    [Route("id/{id}")]
    [ProducesResponseType(typeof(UserProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetById(int id)
    {
        _ = _blobStorageConfig.Value.ProfileContainerUrl ?? throw new ConfigurationErrorsException(nameof(_blobStorageConfig.Value.ProfileContainerUrl));
        _ = _blobStorageConfig.Value.BackgroundProfileContainerUrl ?? throw new ConfigurationErrorsException(nameof(_blobStorageConfig.Value.BackgroundProfileContainerUrl));

        if (int.TryParse(_userManager.GetUserId(User), out int loggedInUserId))
        {
            ApplicationUser? user = await _context.Users.AsNoTracking()
                .Include(user => user.ProfilePhoto)
                .Include(user => user.BackgroundPhoto)
                .Include(user => user.Followee)
                .Include(user => user.Posts)
                .ThenInclude(post => post.Reactions
                    .Where(reaction => reaction.UserId == loggedInUserId))
                .SingleOrDefaultAsync(x => x.Id == id);
            if (user is not null)
            {
                var userProfile = UserProfileDto.FromUser.Compile().Invoke(user, loggedInUserId, _blobStorageConfig.Value.ProfileContainerUrl.ToString(), _blobStorageConfig.Value.BackgroundProfileContainerUrl.ToString());
                return Ok(userProfile);
            }

            return NotFound("User does not exist");
        }

        return BadRequest("Cannot get Id of logged in user.");
    }

    /// <summary>
    /// Gets user profile based on the handle, which is a unique string. 
    /// </summary>
    /// <param name="handle"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("handle/{handle}")]
    [ProducesResponseType(typeof(UserProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByHandle(string handle)
    {
        _ = _blobStorageConfig.Value.ProfileContainerUrl ?? throw new ConfigurationErrorsException(nameof(_blobStorageConfig.Value.ProfileContainerUrl));
        _ = _blobStorageConfig.Value.BackgroundProfileContainerUrl ?? throw new ConfigurationErrorsException(nameof(_blobStorageConfig.Value.BackgroundProfileContainerUrl));

        if (int.TryParse(_userManager.GetUserId(User), out int loggedInUserId))
        {
            ApplicationUser? user = await _context.Users.AsNoTracking()
                .Include(user => user.ProfilePhoto)
                .Include(user => user.BackgroundPhoto)
                .Include(user => user.Followee)
                .Include(user => user.Posts)
                .ThenInclude(post => post.Reactions
                    .Where(reaction => reaction.UserId == loggedInUserId))
                .SingleOrDefaultAsync(x => x.UserName == handle);
            if (user is not null)
            {
                var userProfile = UserProfileDto.FromUser.Compile().Invoke(user, loggedInUserId, _blobStorageConfig.Value.ProfileContainerUrl.ToString(), _blobStorageConfig.Value.BackgroundProfileContainerUrl.ToString());
                return Ok(userProfile);
            }

            return NotFound("User does not exist");
        }

        return BadRequest("Cannot get Id of logged in user.");
    }

    /// <summary>
    /// Gets the BLabs of the individual user.  
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="pageSize"></param>
    /// <param name="pageNumber"></param>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    [Route("{userId}/posts")]
    [ProducesResponseType(typeof(CommandResult<Page<PostDtoForBlabFeed>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CommandResult), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetProfileBlabFeed(
        int userId,
        [FromQuery] int pageSize,
        [FromQuery] int pageNumber)
    {
        var command = new GetBlabFeedCommand(userId, pageSize, pageNumber);

        var result = await _profileBlabFeedService.HandleAsync(command);

        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    /// <summary>
    /// Endpoint to edit user profile details.
    /// </summary>
    /// <param name="request"></param>
    [Authorize]
    [HttpPut]
    [Route("profile")]
    [ProducesResponseType(typeof(CommandResult<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CommandResult<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Edit(EditUserRequest request)
    {
        var checkUserIdValid = int.TryParse(_userManager.GetUserId(User), out var loggedInUserId);
        var command = new UpdateUserCommand(
            loggedInUserId, 
            request.UserId, 
            request.Handle, 
            request.DisplayName,
            request.Bio,
            request.ProfilePhoto!,
            request.BackgroundPhoto!);

        var result = await _updateUserService.HandleAsync(command);

        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    /// <summary>
    /// This endpoint is for searching for users.
    /// </summary>
    /// <returns> If successful, returns SearchForUsersDto, which contains the PageNumber, the number of total users found and a list of Users found. </returns>
    [HttpGet]
    [Authorize]
    [Route("search")]
    [ProducesResponseType(typeof(CommandResult<Page<FoundUserDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CommandResult), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Search([FromQuery] int pageNumber, [FromQuery] int pageSize,
        [FromQuery] string searchTerm)
    {
        var command = new SearchForUsersCommand(pageNumber, pageSize, searchTerm);

        var result = await _searchForUsersService.HandleAsync(command);

        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    /// <summary>
    /// Gets user blabs based on the handle, which is a unique string. 
    /// </summary>
    /// <param name="handle"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("blabs/{handle}")]
    [ProducesResponseType(typeof(UserProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBlabsByHandle(string handle)
    {
        _ = _blobStorageConfig.Value.ProfileContainerUrl ?? throw new ArgumentNullException(nameof(handle));
        _ = _blobStorageConfig.Value.BackgroundProfileContainerUrl ?? throw new ArgumentNullException(nameof(handle));

        if (int.TryParse(_userManager.GetUserId(User), out int loggedInUserId))
        {
            ApplicationUser? user = await _context.Users.AsNoTracking()
                .Include(user => user.Posts)
                .ThenInclude(post => post.Reactions
                    .Where(reaction => reaction.UserId == loggedInUserId))
                .SingleOrDefaultAsync(x => x.UserName == handle);
            if (user is not null)
            {
                var userProfile = UserProfileDto.FromUser.Compile().Invoke(user, loggedInUserId,  _blobStorageConfig.Value.ProfileContainerUrl.ToString(), _blobStorageConfig.Value.BackgroundProfileContainerUrl.ToString());
                return Ok(userProfile);
            }

            return NotFound("User does not exist");
        }

        return BadRequest("Cannot get Id of logged in user.");
    }
}
