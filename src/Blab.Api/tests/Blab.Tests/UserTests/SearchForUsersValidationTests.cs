using FluentAssertions;
using Blab.Api.Requests.Users;
using Blab.Api.Validators.Users;
using Blab.DataAccess;
using Blab.Domain.Entities.Security;
using Moq;
using Moq.EntityFrameworkCore;
using Blab.Services.Services.Users.SearchForUsers;

namespace Blab.Tests.UserTests;
public class SearchForUsersValidationTests
{
    private readonly SearchForUsersValidator _searchForUsersValidator;

    public SearchForUsersValidationTests()
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
                UserName = "zahra",
                DisplayName = "zahra",
                Bio = "Hello my name is bob"
            }
        };

        mockDbContext.Setup(dbContext => dbContext.Users).ReturnsDbSet(users);

        _searchForUsersValidator = new SearchForUsersValidator(mockDbContext.Object);

    }   

    [Fact]
    public async Task PageNumber_can_not_be_negative()
    {
        //Arrange
        var testUser = new SearchForUsersCommand(-10,3,"test");
        var expectedResult = false;

        //Act
        var validator = await _searchForUsersValidator.ValidateAsync(testUser);
        var actualResult = validator.IsValid;

        //Assert
        actualResult.Should().Be(expectedResult);
    }

    [Fact]
    public async Task PageSize_can_not_be_negative()
    {
        //Arrange
        var testUser = new SearchForUsersCommand(2, -3, "test");
        var expectedResult = false;

        //Act
        var validator = await _searchForUsersValidator.ValidateAsync(testUser);
        var actualResult = validator.IsValid;

        //Assert
        actualResult.Should().Be(expectedResult);
    }


    [Fact]
    public async Task SearchTerm_cannot_be_empty()
    {
        //Arrange
        var testUser = new SearchForUsersCommand(2, 2, "");
        var expectedResult = false;

        //Act
        var validator = await _searchForUsersValidator.ValidateAsync(testUser);
        var actualResult = validator.IsValid;

        //Assert
        actualResult.Should().Be(expectedResult);
    }

    [Fact]
    public async Task All_properties_should_be_valid()
    {
        //Arrange
        var testUser = new SearchForUsersCommand(0, 2, "zahra");
        var expectedResult = true;

        //Act
        var validator = await _searchForUsersValidator.ValidateAsync(testUser);
        var actualResult = validator.IsValid;

        //Assert
        actualResult.Should().Be(expectedResult);
    }
    [Fact]
    public async Task SearchTerm_can_not_be_one_character()
    {
        //Arrange
        var testUser = new SearchForUsersCommand(1, 2, "z");
        var expectedResult = false;

        //Act
        var validator = await _searchForUsersValidator.ValidateAsync(testUser);
        var actualResult = validator.IsValid;

        //Assert
        actualResult.Should().Be(expectedResult);
    }

    [Fact]
    public async Task PageNumber_can_not_be_greater_than_the_number_of_pages_possible()
    {

        //Arrange
        var testUser = new SearchForUsersCommand(5, 2, "zahra");
        var expectedResult = false;

        //Act
        var validator = await _searchForUsersValidator.ValidateAsync(testUser);
        var actualResult = validator.IsValid;

        //Assert
        actualResult.Should().Be(expectedResult);
    }
}

