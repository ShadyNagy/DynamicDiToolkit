using DynamicDiToolkit.Services;
using DynamicDiToolkit.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DynamicDiToolkit.Extensions;
public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddSingletonDynamicDiToolkitServices(this IServiceCollection services)
	{
		services.AddSingleton<IArdalisSpecificationRepositoryFactory, ArdalisSpecificationRepositoryFactory>();
		services.AddSingleton<IServiceFactory, ServiceFactory>();

		return services;
	}

	public static IServiceCollection AddScopedDynamicDiToolkitServices(this IServiceCollection services)
	{
		services.AddScoped<IArdalisSpecificationRepositoryFactory, ArdalisSpecificationRepositoryFactory>();
		services.AddScoped<IServiceFactory, ServiceFactory>();

		return services;
	}

	public static IServiceCollection AddTransientDynamicDiToolkitServices(this IServiceCollection services)
	{
		services.AddTransient<IArdalisSpecificationRepositoryFactory, ArdalisSpecificationRepositoryFactory>();
		services.AddTransient<IServiceFactory, ServiceFactory>();

		return services;
	}
}
