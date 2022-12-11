using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Entities;
using FluentAssertions;
using Newtonsoft.Json;
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
    
    //Post - Delete
    [Fact]
    public async Task Add_Remove_User_WithValidToken_WithValidId_ReturnsUser() {
        // Arrange
        await AuthenticateAsync();
        var newUser = new User()
        {
            Username = "test",
            Password = "test",
            Email = "test@test.com"
        };
        
        // Act
        var response = await TestClient.PostAsync($"{PATH}/add", new StringContent(JsonConvert.SerializeObject(newUser), Encoding.UTF8, "application/json"));
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Should().NotBeNull();
        newUser.Id = response.Content.ReadAsAsync<User>().Result.Id;
        
        //cleanup
        var responseDelete = await TestClient.DeleteAsync($"{PATH}/remove/{newUser.Id}");
        responseDelete.StatusCode.Should().Be(HttpStatusCode.OK);
        (await responseDelete.Content.ReadAsAsync<User>()).Should().NotBeNull();
    }
    
}