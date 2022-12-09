using System.Diagnostics;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace AlfTest;

//This is just a test of the test framework



// public class UnitTest1 : IClassFixture<WebApplicationFactory<Program>> {
//     [Fact]
//     public void Test1() { }
// }

public class UnitTest1 {
    private readonly HttpClient _client;
    
    public UnitTest1() {
        var factory = new WebApplicationFactory<Program>();
        _client = factory.CreateClient();
    }

    [Fact]
    public void UserNotAutorizedToUseTheApi() {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, "/Greenhouses/1");
        // Act
        var response = _client.SendAsync(request).Result;
        // Assert
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public void UserAuthorizedToUse() {
        //create a header authorization for the _client with a valid key ="ApiKey" and a valid value = "JYP!$jFqqFxmy@TsF6zBNMaSd3Fd&"
        _client.DefaultRequestHeaders.Add("ApiKey", "JYP!$jFqqFxmy@TsF6zBNMaSd3Fd&");
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, "/Greenhouses/1");
        // Act
        var response = _client.SendAsync(request).Result;
        // Assert
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        
    }

    
}