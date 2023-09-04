using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Application;
using Skills;

var configuration = new ConfigurationBuilder().Build();

var builder = Host.CreateApplicationBuilder();

builder.Services.AddHttpClient();

builder.Services.AddApplicationModule(configuration);
builder.Services.AddSkillsModule(configuration);