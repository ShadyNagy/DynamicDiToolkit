using DynamicDiToolkit.Services;
using DynamicDiToolkit.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DynamicDiToolkit.Extensions;
public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddSingletonArdalisSpecificationRepositoryFactoryServices(this IServiceCollection services)
	{
		services.AddSingleton<IArdalisSpecificationRepositoryFactory, ArdalisSpecificationRepositoryFactory>();

		return services;
	}

	public static IServiceCollection AddScopedArdalisSpecificationRepositoryFactoryServices(this IServiceCollection services)
	{
		services.AddScoped<IArdalisSpecificationRepositoryFactory, ArdalisSpecificationRepositoryFactory>();

		return services;
	}

	public static IServiceCollection AddTransientArdalisSpecificationRepositoryFactoryServices(this IServiceCollection services)
	{
		services.AddTransient<IArdalisSpecificationRepositoryFactory, ArdalisSpecificationRepositoryFactory>();

		return services;
	}
}
