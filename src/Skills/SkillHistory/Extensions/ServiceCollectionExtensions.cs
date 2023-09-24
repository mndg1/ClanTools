using JsonFlatFileDataStore;
using Microsoft.Extensions.DependencyInjection;
using Shared;
using SkillHistory.Data;

namespace SkillHistory.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddSkillHistoryModule(this IServiceCollection services)
	{
		services.AddTransient<ISkillHistoryService, SkillHistoryService>();
		services.AddTransient<ISkillHistoryDataService, SkillHistoryDataFileService>();

		services.AddTransient<INamedDataStore, NamedDataStore>(dataStore => new(SkillHistoryDataFileService.FILE_NAME));

		return services;
	}
}
