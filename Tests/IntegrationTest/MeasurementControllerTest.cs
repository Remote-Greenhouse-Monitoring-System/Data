using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Entities;
using FluentAssertions;
using Xunit;

namespace Tests.IntegrationTest;

public class MeasurementControllerTest : IntegrationTest
{
    private readonly string PATH = "/Measurement";
    
    //get measurements

    [Fact]
    public async Task GetMeasurements_InvalidToken_ReturnsUnauthorized()
    {
        //Arrange

        //Act
        var response = await TestClient.GetAsync($"{PATH}/all/1/4");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task GetMeasurements_ValidToken_ReturnMeasurement()
    {
        //Arrange
        await AuthenticateAsync();

        //Act
        var response = await TestClient.GetAsync($"{PATH}/all/1/4");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadAsAsync<ICollection<Measurement>>()).Should().NotBeNull();
    }

    [Fact]
    public async Task GetMeasurements_ValidToken_WithInvalidUserId_ReturnsUserNotFound()
    {
        // Arrange
        await AuthenticateAsync();

        // Act
        var response = await TestClient.GetAsync($"{PATH}/all/678/4");
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadAsAsync<ICollection<Measurement>>()).Should().BeEmpty();
    }

    //get last measurement
    [Fact]
    public async Task GetLastMeasurement_InvalidToken_ReturnsUnauthorized()
    {
        //Arrange

        //Act
        var response = await TestClient.GetAsync($"{PATH}/last/1");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task GetLastMeasurement_ValidToken_ReturnMeasurement()
    {
        //Arrange
        await AuthenticateAsync();

        //Act
        var response = await TestClient.GetAsync($"{PATH}/last/1");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var measurement = await response.Content.ReadAsAsync<Measurement>();
        measurement.Should().NotBeNull();
    }
    
    [Fact]
    public async Task GetLastMeasurement_ValidToken_WithInvalidUserId_ReturnsUserNotFound()
    {
        // Arrange
        await AuthenticateAsync();

        // Act
        var response = await TestClient.GetAsync($"{PATH}/last/215");
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }
    
    //get all per hour
    [Fact]
    public async Task GetAllPerHour_InvalidToken_ReturnsUnauthorized()
    {
        //Arrange

        //Act
        var response = await TestClient.GetAsync($"{PATH}/allPerHours/1/4");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task GetAllPerHour_ValidToken_ReturnMeasurement()
    {
        //Arrange
        await AuthenticateAsync();

        //Act
        var response = await TestClient.GetAsync($"{PATH}/allPerHours/1/4");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadAsAsync<ICollection<Measurement>>()).Should().NotBeNull();
    }
    
    [Fact]
    public async Task GetAllPerHour_ValidToken_WithInvalidUserId_ReturnsUserNotFound()
    {
        // Arrange
        await AuthenticateAsync();

        // Act
        var response = await TestClient.GetAsync($"{PATH}/allPerHours/678/4");
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadAsAsync<ICollection<Measurement>>()).Should().BeEmpty();
    }
    
    //get all per day
    [Fact]
    public async Task GetAllPerDay_InvalidToken_ReturnsUnauthorized()
    {
        //Arrange

        //Act
        var response = await TestClient.GetAsync($"{PATH}/allPerDays/1/4");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task GetAllPerDay_ValidToken_ReturnMeasurement()
    {
        //Arrange
        await AuthenticateAsync();

        //Act
        var response = await TestClient.GetAsync($"{PATH}/allPerDays/1/4");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadAsAsync<ICollection<Measurement>>()).Should().NotBeNull();
    }
    
    [Fact]
    public async Task GetAllPerDay_ValidToken_WithInvalidUserId_ReturnsUserNotFound()
    {
        // Arrange
        await AuthenticateAsync();

        // Act
        var response = await TestClient.GetAsync($"{PATH}/allPerDays/678/4");
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadAsAsync<ICollection<Measurement>>()).Should().BeEmpty();
    }
    
    //get all per month
    [Fact]
    public async Task GetAllPerMonth_InvalidToken_ReturnsUnauthorized()
    {
        //Arrange

        //Act
        var response = await TestClient.GetAsync($"{PATH}/allPerMonth/1/12/2022");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task GetAllPerMonth_ValidToken_ReturnMeasurement()
    {
        //Arrange
        await AuthenticateAsync();

        //Act
        var response = await TestClient.GetAsync($"{PATH}/allPerMonth/1/12/2022");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadAsAsync<ICollection<Measurement>>()).Should().NotBeNull();
    }
    
    [Fact]
    public async Task GetAllPerMonth_ValidToken_WithInvalidUserId_ReturnsUserNotFound()
    {
        // Arrange
        await AuthenticateAsync();

        // Act
        var response = await TestClient.GetAsync($"{PATH}/allPerMonth/678/12/2022");
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadAsAsync<ICollection<Measurement>>()).Should().BeEmpty();
    }
    
    //get all per year
    [Fact]
    public async Task GetAllPerYear_InvalidToken_ReturnsUnauthorized()
    {
        //Arrange

        //Act
        var response = await TestClient.GetAsync($"{PATH}/allPerYear/1/2022");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }  
    
    [Fact]
    public async Task GetAllPerYear_ValidToken_ReturnMeasurement()
    {
        //Arrange
        await AuthenticateAsync();

        //Act
        var response = await TestClient.GetAsync($"{PATH}/allPerYear/1/2022");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadAsAsync<ICollection<Measurement>>()).Should().NotBeNull();
    }
    
    [Fact]
    public async Task GetAllPerYear_ValidToken_WithInvalidUserId_ReturnsUserNotFound()
    {
        // Arrange
        await AuthenticateAsync();

        // Act
        var response = await TestClient.GetAsync($"{PATH}/allPerYear/678/2022");
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadAsAsync<ICollection<Measurement>>()).Should().BeEmpty();
    }
}