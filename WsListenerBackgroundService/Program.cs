using WsListenerBackgroundService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services => { services.AddHostedService<BackgroundListener>(); })
    .Build();

await host.RunAsync();