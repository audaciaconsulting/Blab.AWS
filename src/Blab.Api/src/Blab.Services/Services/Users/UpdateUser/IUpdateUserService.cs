using Blab.Services.Services.Common;

namespace Blab.Services.Services.Users.UpdateUser;
public interface IUpdateUserService : ICommandHandler<UpdateUserCommand, UpdatedUserDto>
{
}
