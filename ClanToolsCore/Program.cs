using Application;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var configuration = new ConfigurationBuilder()
	.AddApplicationModule()
	.Build();

var builder = Host.CreateApplicationBuilder();

builder.Services.AddApplicationModule(configuration);