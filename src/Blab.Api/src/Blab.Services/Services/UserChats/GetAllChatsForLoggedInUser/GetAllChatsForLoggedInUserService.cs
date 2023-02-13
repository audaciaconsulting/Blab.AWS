using System.Globalization;
using Blab.Api.Configuration.Options;
using Blab.DataAccess;
using Blab.Services.Models.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Blab.Services.Services.UserChats.GetAllChatsForLoggedInUser;
public class GetAllChatsForLoggedInUserService : IGetAllChatsForLoggedInUserService
{
    private readonly BlabDbContext _context;
    private readonly IOptions<BlobStorageConfig> _blobStorageConfig;

    public GetAllChatsForLoggedInUserService(BlabDbContext context, IOptions<BlobStorageConfig> blobStorageConfig)
    {
        _context = context;
        _blobStorageConfig = blobStorageConfig;
    }

    public async Task<CommandResult<IEnumerable<GetAllChatsDto>>> HandleAsync(GetAllChatsForLoggedInUserCommand command, CancellationToken cancellationToken = default)
    {
        var chats = await _context.Chats.AsNoTracking()
                                            .Include(chat => chat.UserChats)
                                            .ThenInclude(userChat => userChat.User)
                                            .ThenInclude(user => user.ProfilePhoto)
                                            .Include(chat => chat.Messages)
                                            .Where(chat => chat.UserChats.Any(userChat => userChat.UserId == command.UserId))
                                            .Select(chat => GetAllChatsDto.FromChat.Compile().Invoke(chat, command.UserId, _blobStorageConfig.Value.ProfileContainerUrl.ToString()))
                                            .ToListAsync();

        if (chats == null)
        {
            CommandResult.Failure<List<int>>(new PropertyErrorMessage("userId",
                   string.Format(CultureInfo.InvariantCulture,
                       "No Chats found for user with id: {0}", command.UserId)));
        }

        return new CommandResult<IEnumerable<GetAllChatsDto>>(chats);
    }
}
