using System.Configuration;
using Blab.Api.Configuration.Options;
using Blab.Api.Requests.Posts;
using Blab.Api.Requests.Reactions;
using Blab.Api.Requests.Users;
using Blab.Api.Validators.Posts;
using Blab.DataAccess;
using Blab.Domain.Entities.Security;
using Blab.Domain.Models;
using Blab.Domain.Models.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Blab.Api.Controllers;

/// <summary>
/// Actions relating to Blabs (posts).
/// </summary>
[ApiController]
[Route("api/blab/")]
public class PostController : ControllerBase
{
    private readonly BlabDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IOptions<BlobStorageConfig> _blobStorageConfig;

    /// <summary>
    /// Initializes a new instance of the <see cref="PostController"/> class.
    /// </summary>
    /// <param name="context">Refers to data in the SQL database.</param>
    /// <param name="userManager">Provides the APIs for managing user in a persistence store.</param>
    /// <param name="blobStorageConfig"></param>
    public PostController(BlabDbContext context, UserManager<ApplicationUser> userManager, IOptions<BlobStorageConfig> blobStorageConfig)
    {
        _context = context;
        _userManager = userManager;
        _blobStorageConfig = blobStorageConfig;
    }

    /// <summary>
    /// Endpoint to view a single Blab by Id.
    /// </summary>
    /// <param name="id">refers to the Id of the Blab.</param>
    /// <returns>The individual Blab will be returned, or Bad request stating unable to find (by Id).</returns>
    [HttpGet]
    [Route("{id}")]
    [Authorize]
    [ProducesResponseType(typeof(PostDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(int id)
    {
        _ = _blobStorageConfig.Value.ProfileContainerUrl ?? throw new ConfigurationErrorsException(nameof(_blobStorageConfig.Value.ProfileContainerUrl));
        var checkUserIdValid = int.TryParse(_userManager.GetUserId(User), out int loggedInUserId);
        var post = await _context.Posts.AsNoTracking()
            .Include(post => post.User)
            .ThenInclude(user => user.ProfilePhoto)
            .Include(post => post.Reactions
                .Where(reaction => reaction.UserId == loggedInUserId))
            .SingleOrDefaultAsync(x => x.Id == id);
        if (post is not null)
        {
            var getPost = PostDto.FromPost.Compile().Invoke(post, loggedInUserId, _blobStorageConfig.Value.ProfileContainerUrl.ToString());
            return Ok(getPost);
        }

        return NotFound($"Unable to find a Blab with id:{id}.");
    }

    /// <summary>
    /// Adding a Blab to the database.  <see cref="Post"/>.
    /// </summary>
    /// <param name="addPostRequest">The actions relating to adding a new Blab.</param>
    /// <returns>A new Blab is added if validation passes, bad request thrown if Blab is Blank or if exceeds 256 characters.</returns> 
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(PostDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Add(AddPostRequest addPostRequest)
    {
        _ = _blobStorageConfig.Value.ProfileContainerUrl ?? throw new ConfigurationErrorsException(nameof(_blobStorageConfig.Value.ProfileContainerUrl));
        var checkUserIdValid = int.TryParse(_userManager.GetUserId(User), out int loggedInUserId);
        var validator = new AddPostValidator(_context, loggedInUserId);
        var validationResult = await validator.ValidateAsync(addPostRequest);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var newPost = new Post
        {
            Content = addPostRequest.Content,
            UserId = addPostRequest.UserId,
            CreatedDate = DateTime.Now
        };

        _context.Posts.Add(newPost);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { id = newPost.Id }, PostDto.FromPost.Compile().Invoke(newPost, loggedInUserId, _blobStorageConfig.Value.ProfileContainerUrl.ToString()));
    }

    /// <summary>
    /// Deleting a Blab from the database by the Blab Id. The Id must match in order for Blab to be deleted.
    /// </summary>
    /// <param name="id">Refers to the Id of the Blab.</param>
    [HttpDelete]
    [Route("{id}")]
    [Authorize]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeletePost(int id)
    {
        var deletePost = await _context.Posts
            .Include(post => post.Reactions)
            .Include(post => post.Comments)
            .SingleOrDefaultAsync(deletePost => deletePost.Id == id);
        if (deletePost is null)
        {
            return BadRequest($"Blab {id} does not exist.");
        }

        var checkId = int.TryParse(_userManager.GetUserId(User), out int blabUserId);

        if (deletePost.UserId != blabUserId)
        {
            return BadRequest($"You cannot delete this Blab: {id}.");
        }

        _context.Posts.Remove(deletePost);
        await _context.SaveChangesAsync();
        return Ok();
    }
}
