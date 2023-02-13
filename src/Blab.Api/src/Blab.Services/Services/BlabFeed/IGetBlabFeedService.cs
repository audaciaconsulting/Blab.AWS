using Blab.Services.Services.Common;
using Audacia.Core;

namespace Blab.Services.Services.BlabFeed;
public interface IGetBlabFeedService : ICommandHandler<GetBlabFeedCommand, Page<PostDtoForBlabFeed>>
{
}
