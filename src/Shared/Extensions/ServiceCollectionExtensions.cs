using Microsoft.Extensions.DependencyInjection;

namespace Shared.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddShared(this IServiceCollection services)
	{
		services.AddSingleton<ITimeProvider, SystemTimeProvider>();

		return services;
	}
}
