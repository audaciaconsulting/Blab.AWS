using Blab.Services.Services.Common;

namespace Blab.Services.Services.Follows.AddFollow;

public interface IAddFollowService : ICommandHandler<AddFollowCommand, bool>
{
}
