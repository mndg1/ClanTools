using JsonFlatFileDataStore;
using Microsoft.Extensions.DependencyInjection;
using Shared;
using Skillathon.Data;
using Skillathon.Processing;
using Skillathon.Publishing;

namespace Skillathon.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddSkillathonModule(this IServiceCollection services)
	{
		services.AddHostedService<SkillathonUpdateWorker>();

		services.AddTransient<ISkillathonService, SkillathonService>();
		services.AddTransient<ISkillathonDataStore, SkillathonFileDataStore>();
		services.AddTransient<ISkillathonPublisher, SkillathonPublisher>();
		services.AddTransient<ISkillathonProcessor, SkillathonProcessor>();

		services.AddTransient<INamedDataStore, NamedDataStore>(dataStore => new(SkillathonFileDataStore.FILE_NAME));

		return services;
	}
}
