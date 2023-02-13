using Blab.Api.Requests.Reactions;
using Blab.DataAccess;
using Blab.Services.Models.Validation;
using Blab.Services.Services.Reactions.AddReaction;
using Blab.Services.Services.Reactions.DeleteReaction;
using Blab.Services.Services.Reactions.UpdateReaction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blab.Api.Controllers;

/// <summary>
/// Actions relating to reacting to a blab/post.
/// </summary>
[ApiController]
[Route("api/blab/")]
public class ReactController : ControllerBase
{
    private readonly BlabDbContext _context;

    private readonly IAddReactionService _addReactionService;

    private readonly IUpdateReactionService _updateReactionService;

    private readonly IDeleteReactionService _deleteReactionService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ReactController"/> class.
    /// </summary>
    /// <param name="context">Refers to data in the SQL database.</param>
    /// <param name="addReactionService">Service for adding a reaction to a post</param>
    /// <param name="updateReactionService">Service for updating a reaction to a post</param>
    /// <param name="deleteReactionService">Service for deleting a reaction to a post</param>
    public ReactController(BlabDbContext context, IAddReactionService addReactionService, IUpdateReactionService updateReactionService, IDeleteReactionService deleteReactionService)
    {
        _context = context;
        _addReactionService = addReactionService;
        _updateReactionService = updateReactionService;
        _deleteReactionService = deleteReactionService;
    }

    /// <summary>
    /// Endpoint to add reaction to a post.
    /// </summary>
    /// <param name="request"></param>
    [Authorize]
    [HttpPost]
    [Route("react")]
    [ProducesResponseType(typeof(CommandResult<int>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(CommandResult), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Add(AddReactionRequest request)
    {
        var command = new AddReactionCommand(request.PostId, request.UserId, request.ReactionType);

        var result = await _addReactionService.HandleAsync(command);

        if (result.IsSuccess)
        {
            return CreatedAtAction("Add", result);
        }

        return BadRequest(result);
    }

    /// <summary>
    /// Endpoint for updating reaction to a post.
    /// </summary>
    /// <param name="request"></param>
    [Authorize]
    [HttpPut]
    [Route("react")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CommandResult), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(UpdateReactionRequest request)
    {
        var command = new UpdateReactionCommand(request.PostId, request.UserId, request.ReactionType);

        var result = await _updateReactionService.HandleAsync(command);

        if (result.IsSuccess)
        {
            return Ok();
        }

        return BadRequest(result);
    }

    /// <summary>
    /// Endpoint for deleting reaction to a post.
    /// </summary>
    /// <param name="request"></param>
    [Authorize]
    [HttpDelete]
    [Route("react")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(CommandResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromQuery] DeleteReactionRequest request)
    {
        var command = new DeleteReactionCommand(request.PostId, request.UserId);

        var result = await _deleteReactionService.HandleAsync(command);

        if (result.IsSuccess)
        {
            return NoContent();
        }

        return NotFound(result);
    }

    /// <summary>
    /// Gets the blab by ID and shows reactions with the count.
    /// </summary>
    /// <param name="id"> Refers to id of the Blab for which the reactions will be returned. </param>
    /// <returns> Counts of reactions to the Blab. </returns>
    [HttpGet]
    [Route("{id}/reactions")]
    [Authorize]
    [ProducesResponseType(typeof(IEnumerable<ReactionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(int id)
    {
        var post = await _context.Posts.AsNoTracking()
            .SingleOrDefaultAsync(post => post.Id == id);
        if (post is not null)
        {
            var getCountsOfReactionsOfTheBlab = await _context.Reactions
                    .Where(reactions => reactions.PostId == id)
                    .GroupBy(reactions => reactions.ReactionType)
                    .Select(ReactionDto.FromReact)
                    .ToListAsync();

            return Ok(getCountsOfReactionsOfTheBlab);
        }

        return NotFound($"Unable to find a Blab with id:{id}.");
    }
}