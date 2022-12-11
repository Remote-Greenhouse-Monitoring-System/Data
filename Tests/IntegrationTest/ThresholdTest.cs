using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Entities;
using FluentAssertions;
using Xunit;

namespace Tests.IntegrationTest;

public class ThresholdTest : IntegrationTest
{
    private readonly string PATH = "/Thresholds";
    
    [Fact]
    public async Task GetThresholdForPlantProfile_InvalidToken_ReturnsUnauthorized() {
        // Arrange
        
        //Act
        var response = await TestClient.GetAsync($"{PATH}/get/1");
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task GetThresholdForPlantProfile_ValidToken_ValidID_ReturnThreshold() {
        // Arrange
        await AuthenticateAsync();
        
        //Act
        var response = await TestClient.GetAsync($"{PATH}/get/1");
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        //response should be equal to an object of type Threshold
        response.Content.Should().NotBeNull();
        (response.Content.ReadAsAsync<Threshold>()).Should().NotBeNull();
    }
    
    
    [Fact]
    public async Task GetThresholdForPlantProfile_ValidToken_InvalidID_ReturnsNotFound() {
        // Arrange
        await AuthenticateAsync();
        
        //Act
        var response = await TestClient.GetAsync($"{PATH}/get/231");
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        
        //response should be equal to an object of type Threshold
    }
    
    
    [Fact]
    public async Task UpdateThresholdForPlantProfile_ValidToken_ValidID_ReturnThreshold() {
        // Arrange
        await AuthenticateAsync();
        
        //Act
        var response = await TestClient.GetAsync($"{PATH}/activeThreshold/1");
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        //response should be equal to an object of type Threshold
        response.Content.Should().NotBeNull();
        (response.Content.ReadAsAsync<Threshold>()).Should().NotBeNull();
    }


    [Fact]
    public async Task UpdateThresholdForPlantProfile_ValidToken_InvalidID_ReturnThreshold() {
        // Arrange
        await AuthenticateAsync();

        //Act
        var response = await TestClient.GetAsync($"{PATH}/activeThreshold/1434");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }
    
}