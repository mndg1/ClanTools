using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Application;
using Skills;

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

var host = builder.Build();

var skillService = host.Services.GetRequiredService<ISkillService>();
var skillSet = await skillService.GetSkillSetAsync("TheKap27");

Console.WriteLine(skillSet.GetSkill("Overall").Level);