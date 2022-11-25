using Contracts;
using EFCData;
using Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using WebSocketClients.Clients;
using WebSocketClients.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<GreenhouseContext>();
builder.Services.AddScoped<IMeasurementService, MeasurementDAO>();
builder.Services.AddScoped<IUserService, UserDAO>();
builder.Services.AddScoped<IMeasurementClient, MeasurementClient>();
builder.Services.AddScoped<IPlantProfileService, PlantProfileDAO>();


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