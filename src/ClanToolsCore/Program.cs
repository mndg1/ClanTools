using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Application;
using SkillHistory.Extensions;
using Application.Extensions;
using Skills.Extensions;
using UserIdentification.Extensions;
using ConsoleApp.Extensions;
using MassTransit;
using Skillathon;
using Skillathon.Extensions;
using Shared.Extensions;

var configuration = new ConfigurationBuilder()
	.AddJsonFile("appsettings.json")
	.Build();

var builder = Host.CreateApplicationBuilder();

builder.Services.AddLogging(loggerBuilder =>
{
	loggerBuilder.ClearProviders();

	var logger = new LoggerConfiguration()
		.WriteTo.Console()
		.CreateLogger();

	loggerBuilder.AddSerilog(logger);
});

builder.Services.AddHttpClient();

builder.Services.AddMassTransit(busConfig =>
{
	busConfig.UsingInMemory();
});

builder.Services.AddShared();

builder.Services.AddUserIdentificationModule();
builder.Services.AddApplicationModule(configuration);

builder.Services.AddSkillsModule(configuration);
builder.Services.AddSkillHistoryModule();
builder.Services.AddSkillathonModule();

builder.Services.AddConsoleApplicationModule();

var host = builder.Build();

var appManager = host.Services.GetRequiredService<IApplicationManager>();
await appManager.StartApplicationsAsync();

var skillathonService = host.Services.GetRequiredService<ISkillathonService>();
var skillathon = await skillathonService.CreateSkillathonAsync("test", "Fishing");
skillathon.RegisterParticipant("TheKap27");
skillathon.StartDate = DateOnly.FromDateTime(DateTime.UtcNow);
await skillathonService.UpdateSkillathonAsync(skillathon);

await host.RunAsync();