using Audacia.Commands;

namespace Blab.Services.Services.Comments.SearchComments;
public class SearchCommentsCommand : ICommand
{
    public int PostId { get; }

    public int PageSize { get; }

    public int PageNumber { get; }

    public SearchCommentsCommand(int postId, int pageSize, int pageNumber)
    {
        PostId = postId;
        PageSize = pageSize;
        PageNumber = pageNumber;
    }
}
