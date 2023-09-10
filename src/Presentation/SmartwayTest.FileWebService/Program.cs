using Microsoft.OpenApi.Models;
using SmartwayTest.FileWebService.Hubs;
using SmartwayTest.DAL.PostgreSQL;
using SmartwayTest.Application;
using SmartwayTest.FileWebService.Configs;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
string dbConnectionString = DbConfig.GetConnectionString(configuration);
var fileStorageOptions = FileStorageConfig.GetFileStorageOptions(configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors();
builder.Services.AddSwaggerGen(options =>
{
    string basePath = AppContext.BaseDirectory;
    string xmlPath = Path.Combine(basePath, "API.xml");
    options.IncludeXmlComments(xmlPath);
    options.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Title = "Smartway.Test.FileWebService API",
            Version = "v1",
            Contact = new OpenApiContact
            {
                Name = "Telegram",
                Url = new Uri("https://t.me/skuld073i")
            }
        }
    );
});
builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
builder.Services.AddSingleton(fileStorageOptions);

builder.Services.AddPostgresDb(dbConnectionString);
builder.Services.AddApplication();
builder.Services
    .AddSignalR()
    .AddJsonProtocol(
        options => options.PayloadSerializerOptions.Converters.Add(new JsonStringEnumConverter())
    );

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(
    builder =>
        builder
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .SetIsOriginAllowed((host) => true)
);

app.MapControllers();
app.MapHubs();

app.Run();
