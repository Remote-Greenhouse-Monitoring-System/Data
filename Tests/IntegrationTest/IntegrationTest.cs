using System.Net.Http;
using System.Threading.Tasks;
using EFCData;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Tests.IntegrationTest; 

public class IntegrationTest {
    protected readonly HttpClient TestClient;

    protected IntegrationTest() {
        var appFactory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder => {
                builder.ConfigureServices(services => {
                    services.RemoveAll(typeof(GreenhouseSystemContext));
                    services.AddDbContext<GreenhouseSystemContext>(options => {
                        options.UseInMemoryDatabase("TestDb");
                    });
                });
            });
        TestClient = appFactory.CreateClient();
    }

    protected async Task AuthenticateAsync() {
        TestClient.DefaultRequestHeaders.Add("ApiKey", "JYP!$jFqqFxmy@TsF6zBNMaSd3Fd&");
    }
}