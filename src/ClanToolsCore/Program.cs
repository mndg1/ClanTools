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

builder.Services.AddApplicationModule(configuration);
builder.Services.AddSkillsModule(configuration);
builder.Services.AddSkillHistoryModule();
builder.Services.AddUserIdentificationModule();

builder.Services.AddConsoleApplicationModule();

var host = builder.Build();

var appManager = host.Services.GetRequiredService<IApplicationManager>();
await appManager.StartApplicationsAsync();