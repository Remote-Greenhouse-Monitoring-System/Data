using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

namespace AlfonsTest;

public class UnitTest1 {
    
    private readonly HttpClient _client;
    
    public UnitTest1() {
        var appFactory = new WebApplicationFactory<GreenhouseDataAPI.Progrem>();
        _client = appFactory.CreateClient();
    }

    [Fact]
    public void Test1() {
        
    }
}