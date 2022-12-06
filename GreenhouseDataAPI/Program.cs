using Contracts;
using EFCData;
using GreenhouseDataAPI;
using WebSocketClients.Clients;
using WebSocketClients.Interfaces;
using WsListenerBackgroundService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<GreenhouseSystemContext>();
builder.Services.AddDbContext<GreenhouseSystemContext>();
builder.Services.AddScoped<IMeasurementService, MeasurementDao>();
builder.Services.AddScoped<IUserService, UserDao>();
builder.Services.AddScoped<IGreenHouseService, GreenHouseDao>();
builder.Services.AddScoped<IPlantProfileService, PlantProfileDao>();
builder.Services.AddScoped<IThresholdService, ThresholdDao>();

//WS-client
builder.Services.AddScoped<IGreenhouseClient, GreenhouseClient>();

// builder.Services.AddHostedService(sp=>sp.GetService<BackgroundListener>());
// builder.Services.AddSingleton<BackgroundListener>();
builder.Services.AddHostedService<BackgroundListener>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();


//websockets:
// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/websockets?view=aspnetcore-7.0
// this is how keepalive interval can be set (default is 2mins)
var webSocketOptions = new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromMinutes(2)
};
//specify allowed request origins
// webSocketOptions.AllowedOrigins.Add("https://client.com");
app.UseWebSockets(webSocketOptions);


app.MapControllers();

app.Run();