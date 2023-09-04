using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Skills.Data;

namespace Skills;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddSkillsModule(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddOptions<SkillsConfiguration>()
			.Bind(configuration.GetSection(SkillsConfiguration.SECTION_NAME));

		services.AddSingleton<ISkillService, SkillService>();

		services.AddSkillsData();

		return services;
	}

	private static IServiceCollection AddSkillsData(this IServiceCollection services)
	{
		services.AddTransient<RuneScapeApiService, HiScoresLiteService>();
		services.AddTransient<ISkillDataRetriever, HiScoresLiteService>();

		return services;
	}
}
