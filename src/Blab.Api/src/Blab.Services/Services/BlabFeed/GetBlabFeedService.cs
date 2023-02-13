﻿using Blab.DataAccess;
using Audacia.Core;
using Blab.Api.Configuration.Options;
using Blab.Services.Extensions.FluentValidationExtensions;
using Blab.Services.Models.Validation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Blab.Services.Services.BlabFeed;
public class GetBlabFeedService : IGetBlabFeedService
{
    private readonly BlabDbContext _context;
    private readonly IOptions<BlobStorageConfig> _blobStorageConfig;

    public GetBlabFeedService(BlabDbContext context, IOptions<BlobStorageConfig> blobStorageConfig)
    {
        _context = context;
        _blobStorageConfig = blobStorageConfig;
    }

    public async Task<CommandResult<Page<PostDtoForBlabFeed>>> HandleAsync(GetBlabFeedCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await ValidateCommandAsync(command, cancellationToken);

        if (validationResult.IsValid)
        {
            var pagingRequest = new SortablePagingRequest(command.PageSize, command.PageNumber)
            {
                SortProperty = nameof(PostDtoForBlabFeed.DateCreated),
                Descending = true
            };

            var blabsForBlabFeed = _context.Posts.AsNoTracking()
                .Include(post => post.Reactions.Where(reaction => reaction.UserId == command.UserId))
                .Include(post => post.User)
                .ThenInclude(user => user.ProfilePhoto)
                .Where(post => post.UserId == command.UserId || post.User.Followee.Any(followee => followee.FollowerId == command.UserId))
                .AsEnumerable()
                .Select(post => PostDtoForBlabFeed.FromPost.Compile().Invoke(post, command.UserId, _blobStorageConfig.Value.ProfileContainerUrl.ToString()));

            var page = new Page<PostDtoForBlabFeed>(blabsForBlabFeed, pagingRequest);

            return new CommandResult<Page<PostDtoForBlabFeed>>(page);
        }

        return CommandResult.Failure<Page<PostDtoForBlabFeed>>(validationResult.Errors.ErrorMessages().ToList());
    }

    private async Task<ValidationResult> ValidateCommandAsync(GetBlabFeedCommand command, CancellationToken cancellationToken)
    {
        var validator = new GetBlabFeedValidator(_context, false);
        return await validator.ValidateAsync(command, cancellationToken);
    }
}
