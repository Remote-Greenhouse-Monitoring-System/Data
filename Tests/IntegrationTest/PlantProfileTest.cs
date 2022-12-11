using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Entities;
using FluentAssertions;
using Xunit;

namespace Tests.IntegrationTest; 

public class PlantProfileTest : IntegrationTest{
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
    public async Task GetActivePlantProfileOnGreenhouse_WhitValidToken_WithIvalidId_ReturnsNotFound(){
        // Arrange
        await AuthenticateAsync();

        // Act
        var response = await TestClient.GetAsync($"{PATH}/activated/1321");
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}