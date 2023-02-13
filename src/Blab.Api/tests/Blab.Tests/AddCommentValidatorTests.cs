using Blab.DataAccess;
using Blab.Domain.Entities.Security;
using Blab.Domain.Models.Comments;
using Blab.Domain.Models.Posts;
using Blab.Services.Services.Comments.AddComment;
using FluentAssertions;
using Moq;
using Moq.EntityFrameworkCore;

namespace Blab.Tests;
public class AddCommentValidatorTests
{
    private readonly AddCommentValidator _addCommentValidator;

    public AddCommentValidatorTests()
    {
        var mockDbContext = new Mock<IBlabDbContext>();

        var cancellationToken = new CancellationToken();

        var users = new List<ApplicationUser>
        {
            new()
            {
                Id = 1,
                UserName = "john.wayne",
                DisplayName = "John",
                Bio = "Hello World"
            },
            new()
            {
                Id = 2,
                UserName = "steve.jobs",
                DisplayName = "steve",
                Bio = "this is the bio"
            },
            new()
            {
                Id = 3,
                UserName = "testUser3",
                DisplayName = "Bob",
                Bio = "Hello my name is bob"
            }
        };

        var posts = new List<Post>
        {
            new()
            {
                Id = 1,
                Content = "test post 1",
                CreatedDate = new DateTime(2000, 01, 01),
                UserId = 1,
            },
            new()
            {
                Id = 2,
                Content = "test post 2",
                CreatedDate = new DateTime(2000, 01, 01),
                UserId = 2,
            },
            new()
            {
                Id = 3,
                Content = "test post 3",
                CreatedDate = new DateTime(2000, 01, 01),
                UserId = 3,
            },
        };

        mockDbContext.Setup(dbContext => dbContext.Users).ReturnsDbSet(users);
        mockDbContext.Setup(dbContext => dbContext.Posts).ReturnsDbSet(posts);

        _addCommentValidator = new AddCommentValidator(mockDbContext.Object, cancellationToken = default);
    }

    [Fact]
    public async Task Test_a_valid_comment_command()
    {
        //Arrange
        var input = new AddCommentCommand(3, 1, "test comment 1");

        //Act
        var validationResult = await _addCommentValidator.ValidateAsync(input);
        var result = validationResult.IsValid;

        //Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task Test_a_invalid_userId()
    {
        //Arrange
        var input = new AddCommentCommand(3, 5, "test comment 2");

        //Act
        var validationResult = await _addCommentValidator.ValidateAsync(input);
        var result = validationResult.IsValid;

        //Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task Test_a_invalid_postId()
    {
        //Arrange
        var input = new AddCommentCommand(5, 2, "test comment 3");

        //Act
        var validationResult = await _addCommentValidator.ValidateAsync(input);
        var result = validationResult.IsValid;

        //Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task Test_a_invalid_content_length()
    {
        //Arrange
        var input = new AddCommentCommand(2, 3, "");

        //Act
        var validationResult = await _addCommentValidator.ValidateAsync(input);
        var result = validationResult.IsValid;

        //Assert
        result.Should().BeFalse();
    }
}
