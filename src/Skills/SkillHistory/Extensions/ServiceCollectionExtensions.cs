using JsonFlatFileDataStore;
using Microsoft.Extensions.DependencyInjection;
using SkillHistory.Data;

namespace SkillHistory.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddSkillHistoryModule(this IServiceCollection services)
	{
		services.AddTransient<ISkillHistoryService, SkillHistoryService>();
		services.AddSingleton<ISkillHistoryDataService, SkillHistoryDataFileService>();

		services.AddSingleton<IDataStore, DataStore>(dataStore => new(SkillHistoryDataFileService.FILE_NAME));

		return services;
	}
}
