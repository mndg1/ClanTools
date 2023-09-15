using Application;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApp.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConsoleApplicationModule(this IServiceCollection services)
    {
        services.AddTransient<IApplication, ConsoleApplication>();

        return services;
    }
}
