using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using SkillHistory.Extensions;
using Application.Extensions;
using Skills.Extensions;
using UserIdentification.Extensions;

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

builder.Services.AddApplicationModule(configuration);
builder.Services.AddSkillsModule(configuration);
builder.Services.AddSkillHistoryModule();
builder.Services.AddUserIdentificationModule();

var host = builder.Build();