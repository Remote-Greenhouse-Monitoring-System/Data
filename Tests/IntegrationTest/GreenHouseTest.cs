using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Entities;
using FluentAssertions;
using Xunit;

namespace Tests.IntegrationTest; 

public class GreenHouseTest : IntegrationTest {

    [Fact]
    public async Task GetGreenHouses_WithValidUserId_ReturnsGreenHouses() {
        // Arrange
        await AuthenticateAsync();
        
        // Act
        //Hardcoded user id
        var response = await TestClient.GetAsync("/Greenhouses/1");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        //there is just 1 greenhouse in the database for the user with id 1
        (await response.Content.ReadAsAsync<List<GreenHouse>>()).Should().HaveCount(1);
    }
}