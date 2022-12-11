using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Entities;
using FluentAssertions;
using Xunit;

namespace Tests.IntegrationTest; 

public class GreenHouseTest : IntegrationTest {
    
    private readonly string PATH = "/Greenhouses";
    
    
    //GET
    
    [Fact] 
    public async Task GetGreenHouses_InvalidToken_ReturnsUnauthorized() {
        //Arrange
        
        //Act
        var response = await TestClient.GetAsync($"{PATH}/1");
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    

    [Fact]
    public async Task GetGreenHouses_WithValidToken_WithValidUserId_ReturnsGreenHouses() {
        // Arrange
        await AuthenticateAsync();
        
        // Act
        //Hardcoded user id
        var response = await TestClient.GetAsync($"{PATH}/1");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        //there is just 1 greenhouse in the database for the user with id 1
        (await response.Content.ReadAsAsync<List<GreenHouse>>()).Should().HaveCount(2);
    }
    
    [Fact]
    public async Task GetGreenHouses_WithValidToken_WithInvalidUserId_ReturnsNotFound() {
        // Arrange
        await AuthenticateAsync();
        
        // Act
        //Hardcoded user id
        var response = await TestClient.GetAsync($"{PATH}/845");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    
    
    
    [Fact]
    public async Task GetLastMeasurementGreenhouse_WithValidToken_ReturnsGreenHouseLastMessurement() {
        // Arrange
        await AuthenticateAsync();
        
        // Act
        //Hardcoded user id
        var response = await TestClient.GetAsync($"{PATH}/lastMeasurementGreenhouse");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadAsAsync<GreenhouseLastMeasurement>()).Should().NotBeNull();
    }
    
    [Fact]
    public async Task GetGreenhouesWithLastMeasurements_WithValidToken_WitValidUserId_ReturnsGreenHousesWithLastMeasurements() {
        // Arrange
        await AuthenticateAsync();
        
        // Act
        var response = await TestClient.GetAsync($"{PATH}/greenhousesWithLastMeasurements/1");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadAsAsync<List<GreenhouseLastMeasurement>>()).Should().NotBeNull();
    }
    
    
    [Fact]
    public async Task GetGreenhousesWithLastMeasurements_WithValidToken_WitInvalidUserId_ReturnsGreenHousesWithLastMeasurements() {
        // Arrange
        await AuthenticateAsync();
        
        // Act
        var response = await TestClient.GetAsync($"{PATH}/greenhousesWithLastMeasurements/345");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    
}