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

public class GreenHouseTest : IntegrationTest {
    
    private readonly string PATH = "/Greenhouses";
    private readonly int USER_ID = 1;
    private readonly int INVALID_USER_ID = 845;
    
    //GET
    
    [Fact] 
    public async Task GetGreenHouses_InvalidToken_ReturnsUnauthorized() {
        //Arrange
        
        //Act
        var response = await TestClient.GetAsync($"{PATH}/{USER_ID}");
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    

    [Fact]
    public async Task GetGreenHouses_WithValidToken_WithValidUserId_ReturnsGreenHouses() {
        // Arrange
        await AuthenticateAsync();
        
        // Act
        //Hardcoded user id
        var response = await TestClient.GetAsync($"{PATH}/{USER_ID}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        //there is just 1 greenhouse in the database for the user with id 1
        
        //Shoul have more than 1 greenhouse
        (await response.Content.ReadAsAsync<List<GreenHouse>>()).Should().HaveCountGreaterThan(1);
    }
    
    [Fact]
    public async Task GetGreenHouses_WithValidToken_WithInvalidUserId_ReturnsNotFound() {
        // Arrange
        await AuthenticateAsync();
        
        // Act
        //Hardcoded user id
        var response = await TestClient.GetAsync($"{PATH}/{INVALID_USER_ID}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    
    
    
    [Fact]
    public async Task GetLastMeasurementGreenhouse_WithValidToken_ReturnsGreenHouseLastMeasurement() {
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
    public async Task GetGreenHousesWithLastMeasurements_WithValidToken_WitValidUserId_ReturnsGreenHousesWithLastMeasurements() {
        // Arrange
        await AuthenticateAsync();
        
        // Act
        var response = await TestClient.GetAsync($"{PATH}/greenhousesWithLastMeasurements/{USER_ID}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadAsAsync<List<GreenhouseLastMeasurement>>()).Should().NotBeNull();
    }
    
    
    [Fact]
    public async Task GetGreenhousesWithLastMeasurements_WithValidToken_WitInvalidUserId_ReturnsGreenHousesWithLastMeasurements() {
        // Arrange
        await AuthenticateAsync();
        
        // Act
        var response = await TestClient.GetAsync($"{PATH}/greenhousesWithLastMeasurements/{INVALID_USER_ID}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    //Post - Delete
    // [Fact]
    // public async Task Add_Remove_GreenHouse_WithValidToken_WithValidGreenHouse_ReturnsGreenHouse() {
    //     // Arrange
    //     await AuthenticateAsync();
    //     var newGreenHouse = new GreenHouse(name:"Test Greenhouse");
    //     
    //     // Act
    //     var response = await TestClient.PostAsync($"{PATH}/{USER_ID}", new StringContent(JsonConvert.SerializeObject(newGreenHouse), Encoding.UTF8, "application/json"));
    //     
    //     // Assert
    //     response.StatusCode.Should().Be(HttpStatusCode.OK);
    //     response.Content.Should().NotBeNull();
    //     newGreenHouse.Id = response.Content.ReadAsAsync<GreenHouse>().Result.Id;
    //     
    //     //cleanup
    //     var responseDelete = await TestClient.DeleteAsync($"{PATH}/{newGreenHouse.Id}");
    //     responseDelete.StatusCode.Should().Be(HttpStatusCode.OK);
    //     (await responseDelete.Content.ReadAsAsync<GreenHouse>()).Should().NotBeNull();
    // }
    
    /*
    //Update
    [Fact]
    public async Task UpdateGreenHouse_WithValidToken_WithValidGreenHouse_ReturnsGreenHouse() {
        // Arrange
        await AuthenticateAsync();
        var newGreenHouse = new GreenHouse(name:"Test Greenhouse");
        var response = await TestClient.PostAsync($"{PATH}/{USER_ID}", new StringContent(JsonConvert.SerializeObject(newGreenHouse), Encoding.UTF8, "application/json"));
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Should().NotBeNull();
        var greenHouseUpdate = response.Content.ReadAsAsync<GreenHouse>().Result;
        greenHouseUpdate.Name = "Updated Greenhouse";
        
        // Act
        var responseUpdate = await TestClient.PatchAsync($"{PATH}/", new StringContent(JsonConvert.SerializeObject(greenHouseUpdate), Encoding.UTF8, "application/json"));

        // Assert
        responseUpdate.StatusCode.Should().Be(HttpStatusCode.OK);
        (await responseUpdate.Content.ReadAsAsync<GreenHouse>()).Name.Should().Be("Updated Greenhouse");
        
        //Delete the greenhouse
        var responseDelete = await TestClient.DeleteAsync($"{PATH}/{newGreenHouse.Id}");
        responseDelete.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    */
    
}