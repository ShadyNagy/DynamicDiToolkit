using DynamicDiToolkit.Services;
using DynamicDiToolkit.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DynamicDiToolkit.Extensions;
public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddSingletonDynamicDiToolkitServices(this IServiceCollection services)
	{
		services.AddSingleton<IArdalisSpecificationRepositoryFactory, ArdalisSpecificationRepositoryFactory>();

		return services;
	}

	public static IServiceCollection AddScopedDynamicDiToolkitServices(this IServiceCollection services)
	{
		services.AddScoped<IArdalisSpecificationRepositoryFactory, ArdalisSpecificationRepositoryFactory>();

		return services;
	}

	public static IServiceCollection AddTransientDynamicDiToolkitServices(this IServiceCollection services)
	{
		services.AddTransient<IArdalisSpecificationRepositoryFactory, ArdalisSpecificationRepositoryFactory>();

		return services;
	}
}
