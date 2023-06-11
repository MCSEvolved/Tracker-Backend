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

var builder = WebApplication.CreateBuilder(args);

FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile("firebase-adminsdk-token.json")


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

});

builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("IsGuest", policy =>
        policy.RequireAssertion(context =>
            context.User.HasClaim(c => c.Type == "guest" && c.Value == "true") ||
            context.User.HasClaim(c => c.Type == "player" && c.Value == "true") ||
            context.User.HasClaim(c => c.Type == "admin" && c.Value == "true")));

    opt.AddPolicy("IsPlayer", policy =>
        policy.RequireAssertion(context =>
            context.User.HasClaim(c => c.Type == "player" && c.Value == "true") ||
            context.User.HasClaim(c => c.Type == "admin" && c.Value == "true")));

    opt.AddPolicy("IsAdmin", policy =>
        policy.RequireAssertion(context =>
            context.User.HasClaim(c => c.Type == "admin" && c.Value == "true")));

    opt.AddPolicy("IsService", policy =>
        policy.RequireAssertion(context =>
            context.User.HasClaim(c => c.Type == "service" && c.Value == "true") ||
            context.User.HasClaim(c => c.Type == "admin" && c.Value == "true")));
});



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

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();
app.MapHub<ServerHub>("/ws/server");
app.MapHub<ClientHub>("/ws/client");
app.MapHub<AuthorizedClientHub>("ws/client/authorized");

app.Run();

 