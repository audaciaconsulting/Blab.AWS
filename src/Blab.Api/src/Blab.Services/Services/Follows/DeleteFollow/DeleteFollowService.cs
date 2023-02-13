using Blab.Services.Models.Validation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Blab.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Blab.Services.Services.Follows.DeleteFollow;
public class DeleteFollowService : IDeleteFollowService
{
    private readonly BlabDbContext _context;

    public DeleteFollowService(BlabDbContext context)
    {
        _context = context;
    }

    public async Task<CommandResult<bool>> HandleAsync(DeleteFollowCommand command, CancellationToken cancellationToken = default)
    {
        var followItem = await _context.Follows.SingleOrDefaultAsync(x =>
            x.FollowerId == command.LoggedInUserId && x.FolloweeId == command.FolloweeId);
        if (followItem is not null)
        {
            _context.Follows.Remove(followItem);
            await _context.SaveChangesAsync();

            return new CommandResult<bool>(true);
        }

        return CommandResult.Failure<bool>(new PropertyErrorMessage(
            "followeeId",
            string.Format(
                CultureInfo.InvariantCulture,
                "Unable to unfollow user with Id: {0}", command.FolloweeId)));
    }
}
