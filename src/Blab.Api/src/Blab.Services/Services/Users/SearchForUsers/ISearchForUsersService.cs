using Blab.Services.Services.Common;
using Audacia.Core;

namespace Blab.Services.Services.Users.SearchForUsers;
public interface ISearchForUsersService : ICommandHandler<SearchForUsersCommand, Page<FoundUserDto>>
{
}
