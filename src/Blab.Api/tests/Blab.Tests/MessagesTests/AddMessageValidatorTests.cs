using Blab.Api.Validators;
using Moq;
using FluentAssertions;
using Blab.Api.Requests.Messages;
using Blab.Api.Validators.Messages;
using Blab.DataAccess;
using Blab.Domain.Entities.Models;
using Blab.Domain.Entities.Security;
using Microsoft.EntityFrameworkCore;
using Moq.EntityFrameworkCore;

namespace Blab.Tests;

public class AddMessageValidatorTests
{
    private readonly Mock<IBlabDbContext> _mockDbContext;

    public AddMessageValidatorTests()
    {
        _mockDbContext = new Mock<IBlabDbContext>();

        var userChats = new List<UserChats>()
        {
            new() { UserId = 1, ChatId = 1 },
            new() { UserId = 2, ChatId = 1 },
        };

        var chats = new List<Chat>()
        {
            new()
            {
                Id = 1,
                Messages = new List<Message>(),
                UserChats = userChats
            }
        };
        
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

        _mockDbContext.Setup(dbContext => dbContext.Chats).ReturnsDbSet(chats);
        _mockDbContext.Setup(dbContext => dbContext.UserChats).ReturnsDbSet(userChats);
        _mockDbContext.Setup(dbContext => dbContext.Users).ReturnsDbSet(users);
    }

    [Fact]
    public async Task Test_that_a_message_is_not_empty()
    {
        // Arrange
        var chatId = 1;
        var input = new AddMessageRequest
            { ChatId = chatId, UserId = 1, Content = "hello world" };

        var expected = true;
        var validator = new AddMessageValidator(_mockDbContext.Object, chatId);

        // Act
        var validationResult = await validator.ValidateAsync(input);
        var result = validationResult.IsValid;

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public async Task Test_that_an_empty_message_is_not_valid()
    {
        // Arrange
        var chatId = 1;
        var input = new AddMessageRequest
            { Content = "" };

        var expected = false;
        var validator = new AddMessageValidator(_mockDbContext.Object, chatId);

        // Act
        var validationResult = await validator.ValidateAsync(input);
        var result = validationResult.IsValid;

        // Assert
        result.Should().Be(expected);
    }
}
