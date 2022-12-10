using System.Diagnostics;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

namespace Tests.IntegrationTest; 

public class UniTestExample {
    private readonly HttpClient _client;
    
    public UniTestExample() {
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