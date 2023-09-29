using Application;
using ConsoleApplication.Skills.SkillathonEvent;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApplication.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddConsoleApplicationModule(this IServiceCollection services)
	{
		services.AddTransient<IApplication, ConsoleApp>();

		services.AddTransient<ISkillathonUpdateProcessor, SkillathonUpdateProcessor>();
		services.AddTransient<ISkillathonUpdater, SkillathonUpdater>();

		return services;
	}
}
