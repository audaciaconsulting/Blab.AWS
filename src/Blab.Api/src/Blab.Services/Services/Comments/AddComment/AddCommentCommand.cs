using Audacia.Commands;

namespace Blab.Services.Services.Comments.AddComment;
public class AddCommentCommand : ICommand
{
    public int PostId { get; }

    public int UserId { get; }

    public string Content { get; }

    public AddCommentCommand(int postId, int userId, string content)
    {
        PostId = postId;
        UserId = userId;
        Content = content;
    }
}
