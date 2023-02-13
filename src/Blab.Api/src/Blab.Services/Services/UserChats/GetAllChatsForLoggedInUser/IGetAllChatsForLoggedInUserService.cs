using Blab.Services.Services.Common;

namespace Blab.Services.Services.UserChats.GetAllChatsForLoggedInUser;
public interface IGetAllChatsForLoggedInUserService : ICommandHandler<GetAllChatsForLoggedInUserCommand, IEnumerable<GetAllChatsDto>>
{
}
