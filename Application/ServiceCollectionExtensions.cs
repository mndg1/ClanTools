using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ServiceCollectionExtensions
{
	public static IConfigurationBuilder AddApplicationModule(this IConfigurationBuilder builder)
	{
		builder.AddJsonFile("applications.json");

		return builder;
	}

	public static IServiceCollection AddApplicationModule(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddOptions<ApplicationsConfiguration>()
			.Bind(configuration.GetSection(ApplicationsConfiguration.SECTION_NAME));

		services.AddSingleton<IApplicationManager, ApplicationManager>();

		return services;
	}
}
