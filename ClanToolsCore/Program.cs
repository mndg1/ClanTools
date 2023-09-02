using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Application;
using Skills;

var configuration = new ConfigurationBuilder()
	.AddJsonFile("configuration.json")
	.Build();

var builder = Host.CreateApplicationBuilder();

builder.Services.AddHttpClient();

builder.Services.AddApplicationModule(configuration);
builder.Services.AddSkillsModule(configuration);

var host = builder.Build();

var appThing = (IApplicationManager) host.Services.GetRequiredService(typeof(IApplicationManager));
await appThing.StartApplicationsAsync();

var skillThing = (ISkillService) host.Services.GetRequiredService(typeof(ISkillService));
var a = await skillThing.GetSkillSetAsync("TheKap27");

Console.WriteLine(a);