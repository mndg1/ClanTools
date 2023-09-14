using JsonFlatFileDataStore;
using Microsoft.Extensions.DependencyInjection;
using Skillathon.Data;

namespace Skillathon.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddSkillathonModule(this IServiceCollection services)
	{
		services.AddTransient<ISkillathonService, SkillathonService>();
		services.AddSingleton<ISkillathonDataService, SkillathonDataFileService>();

		services.AddSingleton<IDataStore, DataStore>(dataStore => new DataStore(SkillathonDataFileService.FILE_NAME));

		return services;
	}
}
