using FluentAssertions;
using Blab.Api.Requests.Users;
using Blab.DataAccess;
using Blab.Domain.Entities.Security;
using Blab.Services.Services.Users.UpdateUser;
using Moq;
using Moq.EntityFrameworkCore;

namespace Blab.Tests.UserTests;

public class EditUserRequestValidatorTests
{
    private readonly UpdateUserValidator _editUserRequestValidator;

    public EditUserRequestValidatorTests()
    {
        var mockDbContext = new Mock<IBlabDbContext>();

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

        mockDbContext.Setup(dbContext => dbContext.Users).ReturnsDbSet(users);

        _editUserRequestValidator = new UpdateUserValidator(mockDbContext.Object.Users, 1);
    }

    [Fact]
    public async Task Testing_valid_username_displayname_and_bio()
    {
        //Arrange
        var testUser = new UpdateUserCommand(
            1,
            1, 
            "validUsername123", 
            "validDisplayName", 
            "this is the changed bio", 
            new UpdateProfilePhotoRequest());
        var expectedResult = true;

        //Act
        var validator = await _editUserRequestValidator.ValidateAsync(testUser);
        var actualResult = validator.IsValid;

        //Assert
        actualResult.Should().Be(expectedResult);
    }

    [Fact]
    public async Task Testing_non_unique_handle()
    {
        //Arrange
        var testUser = new UpdateUserCommand(
            3, 
            3,
            "steve.jobs",
            "validDisplayName",
            "this is the changed bio",
            new UpdateProfilePhotoRequest());
 
        var expectedResult = false;

        //Act
        var validator = await _editUserRequestValidator.ValidateAsync(testUser);
        var actualResult = validator.IsValid;

        //Assert
        actualResult.Should().Be(expectedResult);
    }

    [Fact]
    public async Task Testing_invalid_handle()
    {
        //Arrange
        var testUser = new UpdateUserCommand(
            1, 
            1,
            "steve?",
           "validDisplayName",
            "this is the changed bio",
            new UpdateProfilePhotoRequest());
        var expectedResult = false;

        //Act
        var validator = await _editUserRequestValidator.ValidateAsync(testUser);
        var actualResult = validator.IsValid;

        //Assert
        actualResult.Should().Be(expectedResult);
    }

    [Fact]
    public async Task Testing_invalid_display_name()
    {
        //Arrange
        var testUser = new UpdateUserCommand(
            1, 
            1,
            "validUsername123",
            "J",
            "this is the changed bio", 
            new UpdateProfilePhotoRequest());
  
        var expectedResult = false;

        //Act
        var validator = await _editUserRequestValidator.ValidateAsync(testUser);
        var actualResult = validator.IsValid;

        //Assert
        actualResult.Should().Be(expectedResult);
    }
}
