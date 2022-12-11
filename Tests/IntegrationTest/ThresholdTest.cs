using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Entities;
using FluentAssertions;
using Xunit;

namespace Tests.IntegrationTest; 

public class ThresholdTest : IntegrationTest{
    private readonly string PATH = "/Thresholds";
    private readonly int PLANTP_ID = 78;
    private readonly int PLANTP_INVALID_ID = 879;
    private readonly int GH_ID = 3;
    private readonly int GH_INVALID_ID = 879;
    
    [Fact]
    public async Task GetThresholdForPlantProfile_InvalidToken_ReturnsUnauthorized() {
        // Arrange
        
        //Act
        var response = await TestClient.GetAsync($"{PATH}/get/{PLANTP_ID}");
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task GetThresholdForPlantProfile_ValidToken_ValidID_ReturnThreshold() {
        // Arrange
        await AuthenticateAsync();
        
        //Act
        var response = await TestClient.GetAsync($"{PATH}/get/{PLANTP_ID}");
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        //response should be equal to an object of type Threshold
        response.Content.Should().NotBeNull();
        (await response.Content.ReadAsAsync<Threshold>()).Should().NotBeNull();
    }
    
    
    [Fact]
    public async Task GetThresholdForPlantProfile_ValidToken_InvalidID_ReturnsNotFound() {
        // Arrange
        await AuthenticateAsync();
        
        //Act
        var response = await TestClient.GetAsync($"{PATH}/get/{PLANTP_INVALID_ID}");
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        
        //response should be equal to an object of type Threshold
    }
    
    
    [Fact]
    public async Task GetThresholdOnActivePlantProfile_ValidToken_ValidID_ReturnThreshold() {
        // Arrange
        await AuthenticateAsync();
        
        //Act
        var response = await TestClient.GetAsync($"{PATH}/activeThreshold/{GH_ID}");
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        //response should be equal to an object of type Threshold
        response.Content.Should().NotBeNull();
        (await response.Content.ReadAsAsync<Threshold>()).Should().NotBeNull();
    }


    [Fact]
    public async Task GetThresholdOnActivePlantProfile_ValidToken_InvalidID_ReturnThreshold() {
        // Arrange
        await AuthenticateAsync();

        //Act
        var response = await TestClient.GetAsync($"{PATH}/activeThreshold/{GH_INVALID_ID}");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }
    
    
}