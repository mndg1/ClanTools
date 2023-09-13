﻿using JsonFlatFileDataStore;
using Microsoft.Extensions.DependencyInjection;
using UserIdentification.Data;

namespace UserIdentification.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddUserIdentificationModule(this IServiceCollection services)
	{
		services.AddTransient<IUserIdenificationService, UserIdentificationService>();
		services.AddSingleton<IUserIdentificationDataService, UserIdentificationDataFileService>();
		services.AddSingleton<IGuidProvider, GuidProvider>();

		services.AddSingleton<IDataStore, DataStore>(dataStore => new DataStore(UserIdentificationDataFileService.FILE_NAME));

		return services;
	}
}
