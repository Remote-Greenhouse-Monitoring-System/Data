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
    private readonly int GH_ID = 1;
    private readonly int INVALID_GH_ID = 845;
    private readonly int AMOUNT = 4;
    private readonly int HOURS = 10;
    private readonly int DAYS = 15;
    private readonly int MONTH = 12;
    private readonly int YEAR = 2022;
    
    //get measurements

    [Fact]
    public async Task GetMeasurements_InvalidToken_ReturnsUnauthorized()
    {
        //Arrange

        //Act
        var response = await TestClient.GetAsync($"{PATH}/all/{GH_ID}/{AMOUNT}");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task GetMeasurements_ValidToken_ReturnMeasurement()
    {
        //Arrange
        await AuthenticateAsync();

        //Act
        var response = await TestClient.GetAsync($"{PATH}/all/{GH_ID}/{AMOUNT}");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadAsAsync<ICollection<Measurement>>()).Should().NotBeNull();
    }

    [Fact]
    public async Task GetMeasurements_ValidToken_WithInvalidGreenHouseId_ReturnsUserNotFound()
    {
        // Arrange
        await AuthenticateAsync();

        // Act
        var response = await TestClient.GetAsync($"{PATH}/all/{INVALID_GH_ID}/{AMOUNT}");
        
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
        var response = await TestClient.GetAsync($"{PATH}/last/{GH_ID}");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task GetLastMeasurement_ValidToken_ReturnMeasurement()
    {
        //Arrange
        await AuthenticateAsync();

        //Act
        var response = await TestClient.GetAsync($"{PATH}/last/{GH_ID}");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var measurement = await response.Content.ReadAsAsync<Measurement>();
        measurement.Should().NotBeNull();
    }
    
    [Fact]
    public async Task GetLastMeasurement_ValidToken_WithInvalidGreenHouseId_ReturnsUserNotFound()
    {
        // Arrange
        await AuthenticateAsync();

        // Act
        var response = await TestClient.GetAsync($"{PATH}/last/{INVALID_GH_ID}");
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }
    
    //get all per hour
    [Fact]
    public async Task GetAllPerHour_InvalidToken_ReturnsUnauthorized()
    {
        //Arrange

        //Act
        var response = await TestClient.GetAsync($"{PATH}/allPerHours/{GH_ID}/{HOURS}");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task GetAllPerHour_ValidToken_ReturnMeasurement()
    {
        //Arrange
        await AuthenticateAsync();

        //Act
        var response = await TestClient.GetAsync($"{PATH}/allPerHours/{GH_ID}/{HOURS}");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadAsAsync<ICollection<Measurement>>()).Should().NotBeNull();
    }
    
    [Fact]
    public async Task GetAllPerHour_ValidToken_WithInvalidGreenHouseId_ReturnsUserNotFound()
    {
        // Arrange
        await AuthenticateAsync();

        // Act
        var response = await TestClient.GetAsync($"{PATH}/allPerHours/{INVALID_GH_ID}/{HOURS}");
        
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
        var response = await TestClient.GetAsync($"{PATH}/allPerDays/{GH_ID}/{DAYS}");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task GetAllPerDay_ValidToken_ReturnMeasurement()
    {
        //Arrange
        await AuthenticateAsync();

        //Act
        var response = await TestClient.GetAsync($"{PATH}/allPerDays/{GH_ID}/{DAYS}");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadAsAsync<ICollection<Measurement>>()).Should().NotBeNull();
    }
    
    [Fact]
    public async Task GetAllPerDay_ValidToken_WithInvalidGreenHouseId_ReturnsUserNotFound()
    {
        // Arrange
        await AuthenticateAsync();

        // Act
        var response = await TestClient.GetAsync($"{PATH}/allPerDays/{INVALID_GH_ID}/{DAYS}");
        
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
        var response = await TestClient.GetAsync($"{PATH}/allPerMonth/{GH_ID}/{MONTH}/{YEAR}");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task GetAllPerMonth_ValidToken_ReturnMeasurement()
    {
        //Arrange
        await AuthenticateAsync();

        //Act
        var response = await TestClient.GetAsync($"{PATH}/allPerMonth/{GH_ID}/{MONTH}/{YEAR}");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadAsAsync<ICollection<Measurement>>()).Should().NotBeNull();
    }
    
    [Fact]
    public async Task GetAllPerMonth_ValidToken_WithInvalidGreenHouseId_ReturnsUserNotFound()
    {
        // Arrange
        await AuthenticateAsync();

        // Act
        var response = await TestClient.GetAsync($"{PATH}/allPerMonth/{INVALID_GH_ID}/{MONTH}/{YEAR}");
        
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
        var response = await TestClient.GetAsync($"{PATH}/allPerYear/{GH_ID}/{YEAR}");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }  
    
    [Fact]
    public async Task GetAllPerYear_ValidToken_ReturnMeasurement()
    {
        //Arrange
        await AuthenticateAsync();

        //Act
        var response = await TestClient.GetAsync($"{PATH}/allPerYear/{GH_ID}/{YEAR}");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadAsAsync<ICollection<Measurement>>()).Should().NotBeNull();
    }
    
    [Fact]
    public async Task GetAllPerYear_ValidToken_WithInvalidGreenHouseId_ReturnsUserNotFound()
    {
        // Arrange
        await AuthenticateAsync();

        // Act
        var response = await TestClient.GetAsync($"{PATH}/allPerYear/{INVALID_GH_ID}/{YEAR}");
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadAsAsync<ICollection<Measurement>>()).Should().BeEmpty();
    }
}