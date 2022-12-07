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
builder.Services.AddDbContext<GreenhouseSystemContext>();
builder.Services.AddScoped<IMeasurementService, MeasurementDAO>();
builder.Services.AddScoped<IUserService, UserDAO>();
builder.Services.AddScoped<IGreenHouseService, GreenHouseDAO>();
builder.Services.AddScoped<IPlantProfileService, PlantProfileDAO>();
builder.Services.AddScoped<IThresholdService, ThresholdDAO>();

//WS-client
builder.Services.AddScoped<IThresholdClient, ThresholdClient>();
//Ws-listener
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