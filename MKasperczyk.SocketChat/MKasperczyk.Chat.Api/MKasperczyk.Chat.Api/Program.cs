using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.WebSockets;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MKasperczyk.Chat.Api.DAL;
using MKasperczyk.Chat.Api.Extensions;
using MKasperczyk.Chat.Api.Features;
using MKasperczyk.Chat.Api.Models;
using MKasperczyk.Chat.Api.Repositories;
using MKasperczyk.Chat.Api.Validators;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text;

var corsAllowOrgins = "_chatCorsAllowOrgins";
var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddCors(options =>
{
    options.AddPolicy(name: corsAllowOrgins,
                      policy =>
                      {
                          // TODO: get correct origin from appsettings
                          policy.AllowAnyOrigin();
                          policy.AllowAnyHeader();
                          policy.AllowAnyMethod();
                      });
});

// SERVICE
services.AddTransient<IUserRepository, UserRepository>();
services.AddTransient<IUnitOfWork, UnitOfWork>();
services.AddScoped<IValidator<TokenRequest>, TokenRequestValidator>();
services.AddScoped<IValidator<RegisterRequest>, RegisterRequestValidator>();
services.AddScoped<IValidator<SendMessageRequest>, SendMessageRequestValidator>();
services.AddDbContextFactory<ChatContext>(options => options.UseNpgsql(configuration.GetConnectionString("ChatDatabase")));
//services.AddWebSocket();
//https://www.youtube.com/watch?v=oti4dU8Pv14
// AUTH
services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true
    };
});
services.AddAuthorization();

var app = builder.Build();
//app.MapSockets()


app.UseCors(corsAllowOrgins);
if (app.Environment.IsDevelopment())
{
    // DB Initializer
    var context = builder.Services.BuildServiceProvider()
        .GetRequiredService<ChatContext>();
    await ChatInitializer.Seed(context);
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

var webSocketOptions = new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromMinutes(2),
};
// https://learn.microsoft.com/pl-pl/aspnet/core/fundamentals/websockets?view=aspnetcore-6.0
webSocketOptions.AllowedOrigins.Add("http://localhost:3000");
app.UseWebSockets(webSocketOptions);
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/ws/")
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
            await Echo(webSocket);
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }
    else
    {
        await next(context);
    }
});


app.MapGet("/", () => "Hello World!").AllowAnonymous();
app.MapGet("/users", GetAllUsersHandler.Handle).AllowAnonymous();
app.MapGet("/users/{userId}", GetUserHandler.Handle).AllowAnonymous();
app.MapGet("/messages", GetMessagesHandler.Handle).AllowAnonymous();
app.MapPost("/message", SendMessageHandler.Handle).AllowAnonymous();
app.MapPost("/user/register", RegisterHandler.Handle);
app.MapPost("/security/token", MKasperczyk.Chat.Api.Features.SecurityTokenHandler.Handle).AllowAnonymous();



app.Run();

static async Task Echo(WebSocket webSocket)
{
    var buffer = new byte[1024 * 4];
    var receiveResult = await webSocket.ReceiveAsync(
        new ArraySegment<byte>(buffer), CancellationToken.None);

    while (!receiveResult.CloseStatus.HasValue)
    {
        await webSocket.SendAsync(
            new ArraySegment<byte>(buffer, 0, receiveResult.Count),
            receiveResult.MessageType,
            receiveResult.EndOfMessage,
            CancellationToken.None);

        receiveResult = await webSocket.ReceiveAsync(
            new ArraySegment<byte>(buffer), CancellationToken.None);
    }

    await webSocket.CloseAsync(
        receiveResult.CloseStatus.Value,
        receiveResult.CloseStatusDescription,
        CancellationToken.None);
}
