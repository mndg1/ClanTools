using JsonFlatFileDataStore;
using Microsoft.Extensions.DependencyInjection;
using Shared;
using SkillathonEvent.Data;
using SkillathonEvent.Modification;
using SkillathonEvent.Processing;
using SkillathonEvent.Publishing;

namespace SkillathonEvent.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddSkillathonModule(this IServiceCollection services)
	{
		services.AddHostedService<SkillathonUpdateWorker>();

		services.AddTransient<ISkillathonService, SkillathonService>();
		services.AddTransient<ISkillathonDataStore, SkillathonFileDataStore>();
		services.AddTransient<ISkillathonPublisher, SkillathonPublisher>();
		services.AddTransient<ISkillathonProcessor, SkillathonProcessor>();

		services.AddTransient<ISkillathonCreator, SkillathonCreator>();
		services.AddTransient<ISkillathonUpdater, SkillathonUpdater>();
		services.AddTransient<ISkillathonEditor, SkillathonEditor>();
		services.AddTransient<ISkillathonDeleter, SkillathonDeleter>();

		services.AddTransient<INamedDataStore, NamedDataStore>(dataStore => new(SkillathonFileDataStore.FILE_NAME));

		return services;
	}
}
