using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WsListenerBackgroundService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services => { services.AddHostedService<BackgroundListener>(); })
    .Build();

await host.RunAsync();