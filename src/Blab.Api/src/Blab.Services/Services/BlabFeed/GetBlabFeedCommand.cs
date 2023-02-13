using Audacia.Commands;

namespace Blab.Services.Services.BlabFeed;
public class GetBlabFeedCommand : ICommand
{
    public int UserId { get; set; }

    public int PageSize { get; set; } = 20;

    public int PageNumber { get; set; } = 0; 

    public GetBlabFeedCommand(int userId, int pageSize, int pageNumber)
    {
        UserId = userId;
        PageSize = pageSize;
        PageNumber = pageNumber;
    }   
}
