using Audacia.Core;
using Blab.Api.Requests.Comments;
using Blab.DataAccess;
using Blab.Services.Models.Validation;
using Blab.Services.Services.Comments.AddComment;
using Blab.Services.Services.Comments.SearchComments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blab.Api.Controllers;

/// <summary>
/// Api Controller for requests relating to commenting on blabs.
/// </summary>
[ApiController]
[Route("api/blab/{postId}/comment")]
public class CommentController : ControllerBase
{
    private readonly BlabDbContext _context;

    private readonly IAddCommentService _addCommentService;

    private readonly ISearchCommentsService _searchCommentsService;

    /// <summary>
    /// Initializes a new instance of the <see cref="CommentController"/> class.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="addCommentService"></param>
    /// <param name="searchCommentsService"></param>
    public CommentController(BlabDbContext context, IAddCommentService addCommentService, ISearchCommentsService searchCommentsService)
    {
        _context = context;
        _addCommentService = addCommentService;
        _searchCommentsService = searchCommentsService;
    }

    /// <summary>
    ///Endpoint to add a comment to a post.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="postId"></param>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(CommandResult<int>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(CommandResult), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Add(AddCommentRequest request, int postId)
    {
        var command = new AddCommentCommand(postId, request.UserId, request.Content);

        var result = await _addCommentService.HandleAsync(command);

        if (result.IsSuccess)
        {
            return CreatedAtAction("Add", result);
        }

        return BadRequest(result);
    }

    /// <summary>
    /// Endpoint to search for comments on a post.
    /// </summary>
    /// <param name="postId"></param>
    /// <param name="pageSize"></param>
    /// <param name="pageNumber"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("search")]
    [ProducesResponseType(typeof(CommandResult<Page<CommentDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CommandResult<Page<CommentDto>>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Search(int postId, [FromQuery] int pageSize, [FromQuery] int pageNumber)
    {
        var command = new SearchCommentsCommand(postId, pageSize, pageNumber);

        var result = await _searchCommentsService.HandleAsync(command);

        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
}
