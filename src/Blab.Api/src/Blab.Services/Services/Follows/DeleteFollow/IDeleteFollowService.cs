using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blab.Services.Services.Common;

namespace Blab.Services.Services.Follows.DeleteFollow;
public interface IDeleteFollowService : ICommandHandler<DeleteFollowCommand, bool>
{
}
