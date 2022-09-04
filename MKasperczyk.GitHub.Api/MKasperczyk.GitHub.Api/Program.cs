using Microsoft.AspNetCore.Mvc;
using MKasperczyk.GitHub.Api.Services.Contract;
using Microsoft.OpenApi.Models;
using MKasperczyk.GitHub.Api.Services;
using MKasperczyk.GitHub.Api.Extensions;
using MKasperczyk.GitHub.Api.Options;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// loging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// custom
builder.Services.AddGitHubServices(configuration);

var app = builder.Build();
app.UseHttpLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.IsDevelopment())
{
    app.MapGet("/", (context) => Task.Run((() => context.Response.Redirect("/swagger/index.html"))));
} else
{
    app.MapGet("/", () => "Welcome!");
}
app.MapGet("/repository/{owner}", async ([FromServices] IGitHubStatsService statsService, [FromRoute] string owner) =>
{
    return await statsService.GetRepositoryStatsByOwnerAsync(owner);
});
app.MapGet("/repositoryWithDetails/{owner}", async ([FromServices] IGitHubService gitHubService, [FromRoute] string owner) =>
{
    return await gitHubService.GetRepositoryDetailInfosByOwnerAsync(owner);
});

app.Run();

public partial class Program { }