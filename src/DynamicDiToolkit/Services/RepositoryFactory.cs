using System;
using System.Linq;
using Ardalis.Specification;
using DynamicDiToolkit.Interfaces;

namespace DynamicDiToolkit.Services;

/// <summary>
/// Represents a factory for creating instances of repositories. This factory is designed to abstract the instantiation
/// of repository instances, allowing for loose coupling and easy dependency injection.
/// </summary>
public class RepositoryFactory : IRepositoryFactory
{
	private readonly IServiceProvider _serviceProvider;

	/// <summary>
	/// Initializes a new instance of the <see cref="RepositoryFactory"/> class with the specified service provider.
	/// </summary>
	/// <param name="serviceProvider">The service provider used to resolve repository instances.</param>
	public RepositoryFactory(IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider;
	}

	/// <summary>
	/// Gets an instance of a repository of type <typeparamref name="T"/>.
	/// </summary>
	/// <typeparam name="T">The type of the repository interface to resolve.</typeparam>
	/// <returns>An instance of the repository of type <typeparamref name="T"/>.</returns>
	/// <exception cref="InvalidOperationException">Thrown if the service for the specified type is not found.</exception>
	public T GetRepository<T>() where T : class
	{
		var serviceType = typeof(IRepositoryBase<T>);
		return (T)_serviceProvider.GetService(serviceType)!
					 ?? throw new InvalidOperationException($"Service for type {typeof(T).Name} not found.");
	}

	/// <summary>
	/// Gets a repository instance for the specified entity name using a generic repository type definition.
	/// </summary>
	/// <param name="genericRepositoryTypeDefinition">The generic repository type definition.</param>
	/// <param name="entityName">The name of the entity.</param>
	/// <returns>A repository instance for the specified entity.</returns>
	public object GetRepository(Type genericRepositoryTypeDefinition, string entityName)
	{
		var entityType = AppDomain.CurrentDomain.GetAssemblies()
			.SelectMany(assembly => assembly.GetTypes())
			.FirstOrDefault(type => type.Name.Equals(entityName, StringComparison.OrdinalIgnoreCase));

		return GetRepositoryInternally(genericRepositoryTypeDefinition, entityType, entityName);
	}

	/// <summary>
	/// Gets a repository instance for the specified entity name within a specific assembly, using a generic repository type definition.
	/// </summary>
	/// <param name="genericRepositoryTypeDefinition">The generic repository type definition.</param>
	/// <param name="entityName">The name of the entity.</param>
	/// <param name="entitiesAssembly">The name of the assembly containing the entity.</param>
	/// <returns>A repository instance for the specified entity.</returns>
	public object GetRepository(Type genericRepositoryTypeDefinition, string entityName, string entitiesAssembly)
	{
		var assembly = AppDomain.CurrentDomain.GetAssemblies()
			.FirstOrDefault(a => a.GetName().Name == entitiesAssembly);
		var entityType = assembly?.GetTypes()
			.FirstOrDefault(t => t.Name.Equals(entityName, StringComparison.OrdinalIgnoreCase));

		return GetRepositoryInternally(genericRepositoryTypeDefinition, entityType, entityName);
	}

	/// <summary>
	/// Gets a repository instance for the specified type.
	/// </summary>
	/// <param name="type">The entity type for which to retrieve the repository.</param>
	/// <returns>A repository instance for the specified type.</returns>
	/// <exception cref="InvalidOperationException">Thrown if the service for the specified type is not found.</exception>
	public object GetRepository(Type type)
	{
		var repoType = typeof(IRepositoryBase<>).MakeGenericType(type);
		var service = _serviceProvider.GetService(repoType);
		if (service == null)
		{
			throw new InvalidOperationException($"Service for type {type.Name} not found.");
		}

		return service;
	}

	private object GetRepositoryInternally(Type genericRepositoryTypeDefinition, Type? entityType, string entityName)
	{
		if (entityType == null)
		{
			throw new InvalidOperationException($"Entity type {entityName} not found.");
		}
		if (!genericRepositoryTypeDefinition.IsGenericTypeDefinition)
		{
			throw new ArgumentException("The provided type must be a generic type definition.", nameof(genericRepositoryTypeDefinition));
		}

		var specificRepositoryType = genericRepositoryTypeDefinition.MakeGenericType(entityType);
		var repository = _serviceProvider.GetService(specificRepositoryType);

		if (repository == null)
		{
			throw new InvalidOperationException($"Repository for entity type {entityName} not found.");
		}

		return repository;
	}
}
