using JsonFlatFileDataStore;
using Microsoft.Extensions.DependencyInjection;
using Shared;
using UserIdentification.Data;

namespace UserIdentification.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddUserIdentificationModule(this IServiceCollection services)
	{
		services.AddTransient<IUserIdentificationService, UserIdentificationService>();
		services.AddTransient<IUserIdentificationDataStore, UserIdentificationDataFileStore>();
		services.AddTransient<IGuidProvider, GuidProvider>();

		services.AddTransient<INamedDataStore, NamedDataStore>(dataStore => new(UserIdentificationDataFileStore.FILE_NAME));

		return services;
	}
}
