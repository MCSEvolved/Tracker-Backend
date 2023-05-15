using JsonSubTypes;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MCST_Controller.SignalRHubs;
using MCST_Controller.Controllers;
using MCST_Message.Domain;
using MCST_Message.Data;
using MCST_Computer.Domain;
using MCST_Computer.Data;
using MCST_Location.Domain;
using MCST_Location.Data;
using MCST_Inventory.Data;
using MCST_Inventory.Domain;
using MCST_Command;
using MCST_Computer;
using MCST_Inventory;
using MCST_Location;
using MCST_Message;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddSignalR();


builder.Services.AddSingleton<IMongoClient, MongoClient>(sp => new MongoClient(builder.Configuration.GetConnectionString("MongoDb")));

builder.Services.AddTransient<MessageService>();
builder.Services.AddTransient<MessageRepository>();

builder.Services.AddTransient<ComputerService>();
builder.Services.AddTransient<ComputerRepository>();

builder.Services.AddTransient<LocationService>();
builder.Services.AddTransient<LocationRepository>();

builder.Services.AddTransient<InventoryService>();
builder.Services.AddTransient<InventoryRepository>();

builder.Services.AddTransient<ICommandController, CommandController>();
builder.Services.AddTransient<IComputerController, ComputerController>();
builder.Services.AddTransient<IInventoryController, InventoryController>();
builder.Services.AddTransient<ILocationController, LocationController>();
builder.Services.AddTransient<IMessageController, MessageController>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 1. Add Authentication Services
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = $"https://{builder.Configuration["Auth0:Domain"]}/";
    options.Audience = builder.Configuration["Auth0:Audience"];
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHub<ServerHub>("/ws/server");
app.MapHub<ClientHub>("/ws/client");
app.MapHub<AuthorizedClientHub>("ws/client/authorized");

app.Run();

 