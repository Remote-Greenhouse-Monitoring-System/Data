using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Tests.IntegrationTest; 

public class IntegrationTest {
    protected readonly HttpClient TestClient;

    protected IntegrationTest() {
        var appFactory = new WebApplicationFactory<Program>();
        
        //TODO fake the DB
        // .WithWebHostBuilder(builder => {
        //     builder.ConfigureServices(services => {
        //         services.RemoveAll(typeof);
        //     });
        // });
        TestClient = appFactory.CreateClient();
    }

    protected async Task AuthenticateAsync() {
        TestClient.DefaultRequestHeaders.Add("ApiKey", "JYP!$jFqqFxmy@TsF6zBNMaSd3Fd&");
        // TestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("ApiKey", await GetJwtAsync());
    }

    private async Task<string?> GetJwtAsync() {
        throw new System.NotImplementedException();
    }
}