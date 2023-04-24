using MCST_Message;
using JsonSubTypes;
using Microsoft.OpenApi.Models;
using MCST_Message.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MCST_Message.Data;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MCST_Computer.Domain;
using MCST_Computer.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddSingleton<IMongoClient, MongoClient>(sp => new MongoClient(builder.Configuration.GetConnectionString("MongoDb")));

builder.Services.AddTransient<MessageService>();
builder.Services.AddTransient<MessageRepository>();

builder.Services.AddTransient<ComputerService>();
builder.Services.AddTransient<ComputerRepository>();

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

app.Run();

 