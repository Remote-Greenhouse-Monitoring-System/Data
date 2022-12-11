using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Entities;
using FluentAssertions;
using Xunit;

namespace Tests.IntegrationTest;

public class PlantProfileTest  : IntegrationTest{
    
    private readonly string PATH = "PlantProfiles";

    // get
    
    [Fact]
    public async Task GetPremadePlantProfiles_InvalidToken_ReturnsUnauthorized(){
        // Arrange
        
        // Act
        var response = await TestClient.GetAsync(PATH);
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task GetPremadePlantProfiles_ValidToken_ReturnsCollectionOfPlantProfile(){
        // Arrange
        await AuthenticateAsync();
        
        // Act
        var response = await TestClient.GetAsync(PATH);
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadAsAsync<ICollection<PlantProfile>>()).Should().NotBeNull();
        // (await response.Content.ReadAsAsync<ICollection<PlantProfile>>()).Should().HaveCountGreaterThan(0);
    }
    
    [Fact]
    public async Task GetUserPlantProfile_WhitValidToken_WithValidUserId_ReturnsPlantProfile(){
        // Arrange
        await AuthenticateAsync();

        // Act
        var response = await TestClient.GetAsync($"{PATH}/plantProfilesForUser/1");
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadAsAsync<ICollection<PlantProfile>>()).Should().NotBeNull();
    }
    
    
    [Fact]
    public async Task GetUserPlantProfile_WhitValidToken_WithInvalidUserId_ReturnsUserNotFound(){
        // Arrange
        await AuthenticateAsync();

        // Act
        var response = await TestClient.GetAsync($"{PATH}/plantProfilesForUser/134");
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task GetPlantProfileById_WhitValidToken_WithValidId_ReturnsPlantProfile(){
        // Arrange
        await AuthenticateAsync();

        // Act
        var response = await TestClient.GetAsync($"{PATH}/PlantP/1");
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadAsAsync<PlantProfile>()).Should().NotBeNull();
    }
    
    [Fact]
    public async Task GetPlantProfileById_WhitValidToken_WithInvalidId_ReturnsNotFound(){
        // Arrange
        await AuthenticateAsync();

        // Act
        var response = await TestClient.GetAsync($"{PATH}/PlantP/431");
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task GetActivePlantProfileOnGreenhouse_WhitValidToken_WithValidId_ReturnsPlantProfile(){
        // Arrange
        await AuthenticateAsync();

        // Act
        var response = await TestClient.GetAsync($"{PATH}/activated/1");
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadAsAsync<PlantProfile>()).Should().NotBeNull();
    }
    
    [Fact]
    public async Task GetActivePlantProfileOnGreenhouse_WhitValidToken_WithInvalidId_ReturnsNotFound(){
        // Arrange
        await AuthenticateAsync();

        // Act
        var response = await TestClient.GetAsync($"{PATH}/activated/1321");
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    // post
    [Fact]
    public async Task PostPlantProfile_WhitValidToken_WithValidPlantProfile_ReturnsPlantProfile()
    {
        // Arrange
        await AuthenticateAsync();
        var plantProfile = new PlantProfile()
        {
            Name = "TestPlantProfile",
            Description = "TestPlantProfile",
            OptimalTemperature = 20,
            OptimalHumidity = 20,
            OptimalCo2 = 20,
            OptimalLight = 20,
        };

        // Act
        var response = await TestClient.PostAsJsonAsync($"{PATH}/add/1", plantProfile);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadAsAsync<PlantProfile>()).Should().NotBeNull();
    }

    [Fact]
    public async Task PostPlantProfile_WhitValidToken_WithInvalidUserId_ReturnsBadRequest()
    {
        // Arrange
        await AuthenticateAsync();
        var plantProfile = new PlantProfile()
        {
            Name = "TestPlantProfile",
            Description = "TestPlantProfile",
            OptimalTemperature = 20,
            OptimalHumidity = 20,
            OptimalCo2 = 20,
            OptimalLight = 20,
        };

        // Act
        var response = await TestClient.PostAsJsonAsync($"{PATH}/add/1321", plantProfile);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }
    
    [Fact]
    public async Task PostPlantProfile_WhitInvalidToken_ReturnsBadRequest()
    {
        // Arrange
        
        var plantProfile = new PlantProfile()
        {
            Name = "TestPlantProfile",
            Description = "TestPlantProfile",
            OptimalTemperature = 20,
            OptimalHumidity = 20,
            OptimalCo2 = 20,
            OptimalLight = 20,
        };

        // Act
        var response = await TestClient.PostAsJsonAsync($"{PATH}/add/1", plantProfile);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    /*
    // patch
    [Fact]
    public async Task UpdatePlantProfile_WhitValidToken_ReturnsPlantProfile()
    {
        // Arrange
        await AuthenticateAsync();
        var updatedPlantProfile = new PlantProfile()
        {
            Name = "TestPlantProfile",
            Description = "TestPlantProfile",
            OptimalTemperature = 20,
            OptimalHumidity = 20,
            OptimalCo2 = 20,
            OptimalLight = 20,
        };

        // Act
        var response = await TestClient.PatchAsync($"{PATH}/update/1", updatedPlantProfile);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadAsAsync<PlantProfile>()).Should().NotBeNull();
    }
    */

    [Fact]
    public async Task ActivatePlantProfile_WhitValidToken_ReturnsOk()
    {
        // Arrange
        await AuthenticateAsync();

        // Act
        var response = await TestClient.PatchAsync($"{PATH}/activate/1/1", null);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task DeactivatePlantProfile_WhitValidToken_ReturnsOk()
    {
        // Arrange
        await AuthenticateAsync();

        // Act
        var response = await TestClient.PatchAsync($"{PATH}/deactivate/1", null);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    // delete
    [Fact]
    public async Task DeletePlantProfile_WhitValidToken_ReturnsOk()
    {
        // Arrange
        await AuthenticateAsync();

        // Act
        var response = await TestClient.DeleteAsync($"{PATH}/remove/1");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}