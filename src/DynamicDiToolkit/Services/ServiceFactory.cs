using System;
using System.Linq;
using Ardalis.Specification;
using DynamicDiToolkit.Interfaces;

namespace DynamicDiToolkit.Services;

/// <summary>
/// Represents a factory for creating instances of repositories. This factory is designed to abstract the instantiation
/// of service instances, allowing for loose coupling and easy dependency injection.
/// </summary>
public class ServiceFactory : IServiceFactory
{
	private readonly IServiceProvider _serviceProvider;

	/// <summary>
	/// Initializes a new instance of the <see cref="ServiceFactory"/> class with the specified service provider.
	/// </summary>
	/// <param name="serviceProvider">The service provider used to resolve service instances.</param>
	public ServiceFactory(IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider;
	}

	/// <summary>
	/// Gets an instance of a service of type <typeparamref name="T"/>.
	/// </summary>
	/// <typeparam name="T">The type of the service interface to resolve.</typeparam>
	/// <returns>An instance of the service of type <typeparamref name="T"/>.</returns>
	/// <exception cref="InvalidOperationException">Thrown if the service for the specified type is not found.</exception>
	public T GetService<T>() where T : class
	{
		var serviceType = typeof(IServiceBase<T>);
		return (T)_serviceProvider.GetService(serviceType)!
					 ?? throw new InvalidOperationException($"Service for type {typeof(T).Name} not found.");
	}

	/// <summary>
	/// Gets a service instance for the specified entity name using a generic service type definition.
	/// </summary>
	/// <param name="genericServiceTypeDefinition">The generic service type definition.</param>
	/// <param name="entityName">The name of the entity.</param>
	/// <returns>A service instance for the specified entity.</returns>
	public object GetService(Type genericServiceTypeDefinition, string entityName)
	{
		var entityType = AppDomain.CurrentDomain.GetAssemblies()
			.SelectMany(assembly => assembly.GetTypes())
			.FirstOrDefault(type => type.Name.Equals(entityName, StringComparison.OrdinalIgnoreCase));

		return GetServiceInternally(genericServiceTypeDefinition, entityType, entityName);
	}

	/// <summary>
	/// Gets a service instance for the specified entity name within a specific assembly, using a generic service type definition.
	/// </summary>
	/// <param name="genericServiceTypeDefinition">The generic service type definition.</param>
	/// <param name="entityName">The name of the entity.</param>
	/// <param name="entitiesAssembly">The name of the assembly containing the entity.</param>
	/// <returns>A service instance for the specified entity.</returns>
	public object GetService(Type genericServiceTypeDefinition, string entityName, string entitiesAssembly)
	{
		var assembly = AppDomain.CurrentDomain.GetAssemblies()
			.FirstOrDefault(a => a.GetName().Name == entitiesAssembly);
		var entityType = assembly?.GetTypes()
			.FirstOrDefault(t => t.Name.Equals(entityName, StringComparison.OrdinalIgnoreCase));

		return GetServiceInternally(genericServiceTypeDefinition, entityType, entityName);
	}

	/// <summary>
	/// Gets a service instance for the specified type.
	/// </summary>
	/// <param name="type">The entity type for which to retrieve the service.</param>
	/// <returns>A service instance for the specified type.</returns>
	/// <exception cref="InvalidOperationException">Thrown if the service for the specified type is not found.</exception>
	public object GetService(Type type)
	{
		var repoType = typeof(IServiceBase<>).MakeGenericType(type);
		var service = _serviceProvider.GetService(repoType);
		if (service == null)
		{
			throw new InvalidOperationException($"Service for type {type.Name} not found.");
		}

		return service;
	}

	private object GetServiceInternally(Type genericServiceTypeDefinition, Type? entityType, string entityName)
	{
		if (entityType == null)
		{
			throw new InvalidOperationException($"Entity type {entityName} not found.");
		}
		if (!genericServiceTypeDefinition.IsGenericTypeDefinition)
		{
			throw new ArgumentException("The provided type must be a generic type definition.", nameof(genericServiceTypeDefinition));
		}

		var specificServiceType = genericServiceTypeDefinition.MakeGenericType(entityType);
		var service = _serviceProvider.GetService(specificServiceType);

		if (service == null)
		{
			throw new InvalidOperationException($"Service for entity type {entityName} not found.");
		}

		return service;
	}
}
