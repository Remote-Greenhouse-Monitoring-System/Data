using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Entities;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace Tests.IntegrationTest; 

public class PlantProfileTest : IntegrationTest{
    private readonly string PATH = "PlantProfiles";
    private readonly int USER_ID = 1;
    private readonly int INVALID_USER_ID = 8435;
    private readonly int GH_ID = 3;
    
    
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
        var response = await TestClient.GetAsync($"{PATH}/plantProfilesForUser/{USER_ID}");
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadAsAsync<ICollection<PlantProfile>>()).Should().NotBeNull();
    }
    
    
    [Fact]
    public async Task GetUserPlantProfile_WhitValidToken_WithInvalidUserId_ReturnsUserNotFound(){
        // Arrange
        await AuthenticateAsync();

        // Act
        var response = await TestClient.GetAsync($"{PATH}/plantProfilesForUser/{INVALID_USER_ID}");
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task GetPlantProfileById_WhitValidToken_WithValidId_ReturnsPlantProfile(){
        // Arrange
        await AuthenticateAsync();

        // Act
        var response = await TestClient.GetAsync($"{PATH}/PlantP/{USER_ID}");
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadAsAsync<PlantProfile>()).Should().NotBeNull();
    }
    
    [Fact]
    public async Task GetPlantProfileById_WhitValidToken_WithInvalidId_ReturnsNotFound(){
        // Arrange
        await AuthenticateAsync();

        // Act
        var response = await TestClient.GetAsync($"{PATH}/PlantP/{INVALID_USER_ID}");
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task GetActivePlantProfileOnGreenhouse_WhitValidToken_WithValidId_ReturnsPlantProfile(){
        // Arrange
        await AuthenticateAsync();

        // Act
        var response = await TestClient.GetAsync($"{PATH}/activated/{USER_ID}");
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadAsAsync<PlantProfile>()).Should().NotBeNull();
    }
    
    [Fact]
    public async Task GetActivePlantProfileOnGreenhouse_WhitValidToken_WithInvalidId_ReturnsNotFound(){
        // Arrange
        await AuthenticateAsync();

        // Act
        var response = await TestClient.GetAsync($"{PATH}/activated/{INVALID_USER_ID}");
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    //Post - Delete
    [Fact]
    public async Task Add_Remove_PlantProfileOnGreenhouse_WhitValidToken_WithValidId_ReturnsPlantProfile()
    {
        // Arrange
        await AuthenticateAsync();
        var newPlantProfile = new PlantProfile()
        {
            Name = "TestPlantProfile",
            Description = "TestPlantProfile",
            OptimalTemperature = 20,
            OptimalHumidity = 20,
            OptimalCo2 = 20,
            OptimalLight = 20,
        };
        
        // Act
        var response = await TestClient.PostAsync($"{PATH}/add/{USER_ID}", new StringContent(JsonConvert.SerializeObject(newPlantProfile), Encoding.UTF8, "application/json"));
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Should().NotBeNull();
        newPlantProfile.Id = response.Content.ReadAsAsync<PlantProfile>().Result.Id;
        
        //cleanup
        var responseDelete = await TestClient.DeleteAsync($"{PATH}/remove/{newPlantProfile.Id}");
        responseDelete.StatusCode.Should().Be(HttpStatusCode.OK);
        (await responseDelete.Content.ReadAsAsync<PlantProfile>()).Should().NotBeNull();
        
    }
}