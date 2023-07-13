using MongoDB.Driver;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MCST_Controller.SignalRHubs;
using MCST_Message.Domain;
using MCST_Message.Data;
using MCST_Computer.Domain;
using MCST_Computer.Data;
using MCST_Location.Domain;
using MCST_Location.Data;
using MCST_Inventory.Data;
using MCST_Inventory.Domain;
using MCST_ControllerInterfaceLayer.Interfaces;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.IdentityModel.Tokens;
using MCST_Controller.Services;
using MCST_Auth.Domain;
using MCST_Notification.Domain;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile(builder.Configuration["CertificatePath"])


});

// firebase auth
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(opt =>
{
    opt.Authority = builder.Configuration["Jwt:Firebase:ValidIssuer"];
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Firebase:ValidIssuer"],
        ValidAudience = builder.Configuration["Jwt:Firebase:ValidAudience"],
    };
    opt.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];

            // If the request is for our hub...
            // The path validation might need some tweaking.
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) &&
                path.StartsWithSegments("/tracker/ws"))
            {
                // Read the token out of the query string
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };

});

builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("IsGuest", policy =>
        policy.RequireRole(new string[] { "isGuest", "isAdmin", "isPlayer"}));

    opt.AddPolicy("IsPlayer", policy =>
        policy.RequireRole(new string[] { "isAdmin", "isPlayer" }));

    opt.AddPolicy("IsAdmin", policy =>
        policy.RequireRole(new string[] { "isAdmin" }));

    opt.AddPolicy("IsService", policy =>
        policy.RequireRole(new string[] { "isAdmin", "isService" }));

});


// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddSignalR(opt =>
{
    opt.EnableDetailedErrors = true;

})
    .AddJsonProtocol(options =>
    {
        options.PayloadSerializerOptions.Converters
           .Add(new JsonStringEnumConverter());
    }); ;


builder.Services.AddSingleton<IMongoClient, MongoClient>(sp => new MongoClient(builder.Configuration.GetConnectionString("MongoDb")));

builder.Services.AddTransient<MessageService>();
builder.Services.AddTransient<MessageRepository>();

builder.Services.AddTransient<ComputerService>();
builder.Services.AddTransient<ComputerRepository>();

builder.Services.AddTransient<LocationService>();
builder.Services.AddTransient<LocationRepository>();

builder.Services.AddTransient<InventoryService>();
builder.Services.AddTransient<InventoryRepository>();

builder.Services.AddTransient<IWsService, WsService>();

builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<INotificationService, NotificationService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();





var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseCors(options => options
    .SetIsOriginAllowedToAllowWildcardSubdomains()
    .WithOrigins(
        "http://localhost:5173",
        "http://localhost:8000",
        "http://localhost:8080",
        "https://mcsynergy.nl",
        "https://*.mcsynergy.nl",
        "https://josian.nl",
        "https://*.josian.nl"
        )
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials());

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();
app.MapHub<ServerHub>("/tracker/ws/server");
app.MapHub<ClientHub>("/tracker/ws/client");
app.MapHub<AuthorizedClientHub>("/tracker/ws/client/authorized");

app.Run();