using DynamicDiToolkit.Services;
using DynamicDiToolkit.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DynamicDiToolkit.Extensions;

/// <summary>
/// Extension methods for adding DynamicDiToolkit services to the IServiceCollection.
/// </summary>
public static class ServiceCollectionExtensions
{
	/// <summary>
	/// Adds the necessary DynamicDiToolkit services to the IServiceCollection as singletons.
	/// This method should be called during the application startup to configure the required
	/// services for DynamicDiToolkit with singleton lifetime.
	/// </summary>
	/// <param name="services">The IServiceCollection to add the services to.</param>
	/// <returns>The original IServiceCollection instance, for chaining further additions.</returns>
	public static IServiceCollection AddSingletonDynamicDiToolkitServices(this IServiceCollection services)
	{
		services.AddSingleton<IArdalisSpecificationRepositoryFactory, ArdalisSpecificationRepositoryFactory>();
		services.AddSingleton<IServiceFactory, ServiceFactory>();
		services.AddSingleton<ITypeResolver, TypeResolver>();
		services.AddSingleton<INewtonsoftJsonDeserializer, NewtonsoftJsonDeserializer>();
		services.AddSingleton<ISystemTextJsonDeserializer, SystemTextJsonDeserializer>();
		services.AddSingleton(typeof(EntityService<>));

		return services;
	}


	/// <summary>
	/// Adds the necessary DynamicDiToolkit services to the IServiceCollection as scoped services.
	/// This is useful for scenarios where the DynamicDiToolkit services need to have a scoped lifetime,
	/// typically used in web applications to ensure the services are scoped per request.
	/// </summary>
	/// <param name="services">The IServiceCollection to add the services to.</param>
	/// <returns>The original IServiceCollection instance, for chaining further additions.</returns>
	public static IServiceCollection AddScopedDynamicDiToolkitServices(this IServiceCollection services)
	{
		services.AddScoped<IArdalisSpecificationRepositoryFactory, ArdalisSpecificationRepositoryFactory>();
		services.AddScoped<IServiceFactory, ServiceFactory>();
		services.AddScoped<ITypeResolver, TypeResolver>();
		services.AddScoped<INewtonsoftJsonDeserializer, NewtonsoftJsonDeserializer>();
    services.AddScoped<ISystemTextJsonDeserializer, SystemTextJsonDeserializer>();
		services.AddScoped(typeof(EntityService<>));

		return services;
	}

	/// <summary>
	/// Adds the necessary DynamicDiToolkit services to the IServiceCollection as transient services.
	/// This method is ideal for when the DynamicDiToolkit services are needed on a per-operation basis,
	/// ensuring that new instances are created with each service request.
	/// </summary>
	/// <param name="services">The IServiceCollection to add the services to.</param>
	/// <returns>The original IServiceCollection instance, for chaining further additions.</returns>
	public static IServiceCollection AddTransientDynamicDiToolkitServices(this IServiceCollection services)
	{
		services.AddTransient<IArdalisSpecificationRepositoryFactory, ArdalisSpecificationRepositoryFactory>();
		services.AddTransient<IServiceFactory, ServiceFactory>();
		services.AddTransient<ITypeResolver, TypeResolver>();
		services.AddTransient<INewtonsoftJsonDeserializer, NewtonsoftJsonDeserializer>();
		services.AddTransient<ISystemTextJsonDeserializer, SystemTextJsonDeserializer>();
		services.AddTransient(typeof(EntityService<>));

		return services;
	}
}
