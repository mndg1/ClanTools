using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using ConsoleApplication;
using SkillHistory.Extensions;
using Application.Extensions;
using Skills.Extensions;
using SkillathonEvent.Extensions;
using UserIdentification.Extensions;
using MassTransit;
using Shared.Extensions;
using ConsoleApplication.Extensions;

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

builder.Services.AddMassTransit(busRegistrationConfig =>
{
	busRegistrationConfig.AddSkillathonConsumers();

	busRegistrationConfig.AddConsoleApplicationConsumers();

	busRegistrationConfig.UsingInMemory((busRegistrationContext, busFactoryConfig) =>
	{
		busFactoryConfig.ConfigureEndpoints(busRegistrationContext);
	});
});

builder.Services.AddShared();

builder.Services.AddUserIdentificationModule();
builder.Services.AddApplicationModule(configuration);

builder.Services.AddSkillsModule(configuration);
builder.Services.AddSkillHistoryModule();
builder.Services.AddSkillathonModule();

builder.Services.AddConsoleApplicationModule();

builder.Services.AddHostedService<CommandProcessorWorker>();

var host = builder.Build();

await host.RunAsync();