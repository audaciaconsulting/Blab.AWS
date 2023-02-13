using Blab.DataAccess;
using Blab.Domain.Entities.Security;
using Blab.Domain.Models.Comments;
using Blab.Domain.Models.Posts;
using Blab.Services.Services.Comments.SearchComments;
using FluentAssertions;
using Moq;
using Moq.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blab.Tests;
public class SearchCommentsValidatorTests
{
    private readonly SearchCommentsValidator _searchCommentsValidator;

    public SearchCommentsValidatorTests()
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
        };

        var comments = new List<Comment>
        {
            new()
            {
                Id = 1,
                Content = "test comment 1",
                Created = new DateTime(2000, 01, 01),
                PostId = 1,
                UserId = 1,

            },
            new()
            {
                Id = 2,
                Content = "test comment 2",
                Created = new DateTime(2000, 01, 01),
                PostId = 1,
                UserId = 2,
            },
            new()
            {
                Id = 3,
                Content = "test comment 3",
                Created = new DateTime(2000, 01, 01),
                PostId = 1,
                UserId = 3,
            },
            new()
            {
                Id = 4,
                Content = "test comment 4",
                Created = new DateTime(2000, 01, 01),
                PostId = 1,
                UserId = 3,
            },
            new()
            {
                Id = 5,
                Content = "test comment 5",
                Created = new DateTime(2000, 01, 01),
                PostId = 1,
                UserId = 3,
            },
            new()
            {
                Id = 6,
                Content = "test comment 6",
                Created = new DateTime(2000, 01, 01),
                PostId = 1,
                UserId = 3,
            },
        };

        mockDbContext.Setup(dbContext => dbContext.Users).ReturnsDbSet(users);
        mockDbContext.Setup(dbContext => dbContext.Posts).ReturnsDbSet(posts);
        mockDbContext.Setup(dbContext => dbContext.Comments).ReturnsDbSet(comments);

        _searchCommentsValidator = new SearchCommentsValidator(mockDbContext.Object, cancellationToken = default);
    }

    [Fact]
    public async Task Test_a_valid_search_comment_command()
    {
        //Arrange
        var input = new SearchCommentsCommand(1, 4, 0);

        //Act
        var validationResult = await _searchCommentsValidator.ValidateAsync(input);
        var result = validationResult.IsValid;

        //Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task Test_invalid_postId()
    {
        //Arrange
        var input = new SearchCommentsCommand(5, 10, 0);

        //Act
        var validationResult = await _searchCommentsValidator.ValidateAsync(input);
        var result = validationResult.IsValid;

        //Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task Test_page_size_greater_than_number_of_comments()
    {
        //Arrange
        var input = new SearchCommentsCommand(1, 10, 0);

        //Act
        var validationResult = await _searchCommentsValidator.ValidateAsync(input);
        var result = validationResult.IsValid;

        //Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task Test_out_of_range_page_number()
    {
        //Arrange
        var input = new SearchCommentsCommand(1, 10, 2);

        //Act
        var validationResult = await _searchCommentsValidator.ValidateAsync(input);
        var result = validationResult.IsValid;

        //Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task Test_negative_page_number()
    {
        //Arrange
        var input = new SearchCommentsCommand(1, 10, -2);

        //Act
        var validationResult = await _searchCommentsValidator.ValidateAsync(input);
        var result = validationResult.IsValid;

        //Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task Test_zero_page_size()
    {
        //Arrange
        var input = new SearchCommentsCommand(1, 0, 1);

        //Act
        var validationResult = await _searchCommentsValidator.ValidateAsync(input);
        var result = validationResult.IsValid;

        //Assert
        result.Should().BeFalse();
    }
}
