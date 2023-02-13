using Audacia.Core;
using Blab.Services.Services.Common;

namespace Blab.Services.Services.Comments.SearchComments;
public interface ISearchCommentsService : ICommandHandler<SearchCommentsCommand, Page<CommentDto>>
{
}
