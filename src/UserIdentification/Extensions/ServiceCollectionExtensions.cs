using JsonFlatFileDataStore;
using Microsoft.Extensions.DependencyInjection;
using UserIdentification.Data;

namespace UserIdentification.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddUserIdentificationModule(this IServiceCollection services)
	{
		services.AddTransient<IUserIdentificationService, UserIdentificationService>();
		services.AddSingleton<IUserIdentificationDataStore, UserIdentificationDataFileStore>();
		services.AddSingleton<IGuidProvider, GuidProvider>();

		services.AddSingleton<IDataStore, DataStore>(dataStore => new DataStore(UserIdentificationDataFileStore.FILE_NAME));

		return services;
	}
}
