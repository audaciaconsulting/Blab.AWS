using Audacia.Core;
using Blab.Api.Configuration.Options;
using Blab.DataAccess;
using Blab.Services.Extensions.FluentValidationExtensions;
using Blab.Services.Models.Validation;
using Blab.Services.Services.Common;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Blab.Services.Services.Comments.SearchComments;
public class SearchCommentsService : ISearchCommentsService
{
    private readonly BlabDbContext _context;
    private readonly IOptions<BlobStorageConfig> _blobStorageConfig;

    public SearchCommentsService(BlabDbContext context, IOptions<BlobStorageConfig> blobStorageConfig)
    {
        _context = context;
        _blobStorageConfig = blobStorageConfig;
    }

    public async Task<CommandResult<Page<CommentDto>>> HandleAsync(SearchCommentsCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await ValidateCommandAsync(command, cancellationToken);
        if (validationResult.IsValid)
        {
            var pagingRequest = new SortablePagingRequest(command.PageSize, command.PageNumber)
            {
                SortProperty = nameof(CommentDto.Created),
                Descending = true
            };

            var page = new Page<CommentDto>(
                await _context.Comments
                    .AsNoTracking()
                    .Include(comment => comment.User)
                    .ThenInclude(user => user.ProfilePhoto)
                    .Where(comment => comment.PostId == command.PostId)
                    .Select(comment => CommentDto.FromComment.Compile().Invoke(comment, _blobStorageConfig.Value.ProfileContainerUrl.ToString())).ToListAsync(), pagingRequest);

            return new CommandResult<Page<CommentDto>>(page);
        }

        return CommandResult.Failure<Page<CommentDto>>(validationResult.Errors.ErrorMessages().ToList());
    }

    private async Task<ValidationResult> ValidateCommandAsync(SearchCommentsCommand command, CancellationToken cancellationToken)
    {
        var validator = new SearchCommentsValidator(_context, cancellationToken);
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        return validationResult;
    }
}
