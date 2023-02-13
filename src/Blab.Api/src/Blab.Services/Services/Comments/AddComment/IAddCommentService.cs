using Blab.Services.Services.Common;

namespace Blab.Services.Services.Comments.AddComment;
public interface IAddCommentService : ICommandHandler<AddCommentCommand, int>
{
}
