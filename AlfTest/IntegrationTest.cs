using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;

namespace AlfTest; 

public class IntegrationTest {
    protected readonly HttpClient TestClient;

    protected IntegrationTest() {
        var appFactory = new WebApplicationFactory<Program>();
        TestClient = appFactory.CreateClient();
    }

    protected async Task AuthenticateAsync() {
        TestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("ApiKey", await GetJwtAsync());
        
    }

    private async Task<string?> GetJwtAsync() {
        throw new System.NotImplementedException();
    }
}