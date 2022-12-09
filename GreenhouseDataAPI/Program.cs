using Contracts;
using EFCData;
using Microsoft.OpenApi.Models;
using WsListenerBackgroundService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Adds api key authorization in Swagger UI 

// Requires a header with a key-value pair of "ApiKey":"{value}"
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Greenhouse Monitoring System Data Tier API", Version = "v1" });
    
    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "ApiKey must appear in header",
        Type = SecuritySchemeType.ApiKey,
        Name = "ApiKey",
        In = ParameterLocation.Header,
        Scheme = "ApiKeyScheme"
    });
    
    var key = new OpenApiSecurityScheme()
    {
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "ApiKey"
        },
        In = ParameterLocation.Header
    };
    
    var requirement = new OpenApiSecurityRequirement
    {
        { key, new List<string>() }
    };
    
    c.AddSecurityRequirement(requirement);
});
builder.Services.AddScoped<GreenhouseSystemContext>();
builder.Services.AddDbContext<GreenhouseSystemContext>();
builder.Services.AddScoped<IMeasurementService, MeasurementDao>();
builder.Services.AddScoped<IUserService, UserDao>();
builder.Services.AddScoped<IGreenHouseService, GreenHouseDao>();
builder.Services.AddScoped<IPlantProfileService, PlantProfileDao>();
builder.Services.AddScoped<IThresholdService, ThresholdDao>();

//Ws-client
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