using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Entities;
using FluentAssertions;
using Xunit;

namespace Tests.IntegrationTest;

public class UserTest : IntegrationTest {
    
    private readonly string PATH = "Users";
    private readonly int USER_ID = 1;
    private readonly string USER_NAME = "vlad12345";
    private readonly string EMAIL = "vlad@via.dk";
    private readonly string PASSWORD = "12341234";

    [Fact]
    public async Task GetUserByUsername_InvalidToken_ReturnsUnauthorized() {
        // Arrange

        // Act
        var response = await TestClient.GetAsync($"{PATH}/byUsername/{USER_NAME}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

    }

    [Fact]
    public async Task GetUserByUsername_ValidToken_ValidUserName_ReturnsUser() {
        // Arrange
        await AuthenticateAsync();

        // Act
        var response = await TestClient.GetAsync($"{PATH}/byUsername/{USER_NAME}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadAsAsync<User>()).Should().NotBeNull();
    }

    [Fact]
    public async Task GetUserByUsername_ValidToken_InvalidUserName_ReturnsNotFound() {
        // Arrange
        await AuthenticateAsync();

        // Act
        var response = await TestClient.GetAsync($"{PATH}/byUsername/123");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }

    [Fact]
    public async Task GetUserByEmail_ValidToken_ValidEmail_ReturnsUser() {
        // Arrange
        await AuthenticateAsync();

        // Act
        var response = await TestClient.GetAsync($"{PATH}/byEmail/{EMAIL}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadAsAsync<User>()).Should().NotBeNull();
    }

    [Fact]
    public async Task GetUserByEmail_ValidToken_InvalidEmail_ReturnsNotFound() {
        // Arrange
        await AuthenticateAsync();
    
        // Act
        var response = await TestClient.GetAsync($"{PATH}/byEmail/notexist@gmail.com");
    
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }

    [Fact]
    public async Task GetUserById_ValidToken_ValidId_ReturnsUser() {
        // Arrange
        await AuthenticateAsync();

        // Act
        var response = await TestClient.GetAsync($"{PATH}/byId/{USER_ID}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadAsAsync<User>()).Should().NotBeNull();
    }

    [Fact]
    public async Task GetUserById_ValidToken_InvalidId_ReturnsNotFound() {
        // Arrange
        await AuthenticateAsync();

        // Act
        var response = await TestClient.GetAsync($"{PATH}/byId/123");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }

    [Fact]
    public async Task LogUserIn_ValidToken_ValidCredentials_ReturnsUser() {
        // Arrange
        await AuthenticateAsync();

        // Act
        //call the login method from query string
        var response = await TestClient.GetAsync($"{PATH}/login?email={EMAIL}&password={PASSWORD}");
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadAsAsync<User>()).Should().NotBeNull();
    }

    [Fact]
    public async Task LogUserIn_ValidToken_InvalidCredentials_ReturnsNotFound() {
        // Arrange
        await AuthenticateAsync();

        // Act
        //call the login method from query string
        var response = await TestClient.GetAsync($"{PATH}/login?email={EMAIL}&password=123");
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }

    // [Fact]
    // public async Task AddUser_ValidToken_ValidUser_ReturnsUser() {
    //     // Arrange
    //     await AuthenticateAsync();
    //
    //     // Act
    //     // call the Post method with a body new user object as a json
    // }
    //
    // [Fact]
    // async Task RemoveUser_ValidToken_ValidId_ReturnsOk() {
    //     // Arrange
    //     await AuthenticateAsync();
    //
    //     // Act
    //     var response = await TestClient.DeleteAsync($"{PATH}/remove/86");
    //
    //     // Assert
    //     response.StatusCode.Should().Be(HttpStatusCode.OK);
    //
    // }
}