using Audacia.Commands;

namespace Blab.Services.Services.Users.SearchForUsers;
public class SearchForUsersCommand : ICommand
{
    public int PageNumber { get; set; } = 0;

    public int PageSize { get; set; } = 5;

    public string SearchTerm { get; set; } 

    public SearchForUsersCommand(int pageNumber, int pageSize, string searchTerm)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        SearchTerm = searchTerm;
    }
}
