using System;
using System.Linq;
using Ardalis.Specification;
using DynamicDiToolkit.Interfaces;

namespace DynamicDiToolkit.Services;

public class ArdalisSpecificationRepositoryFactory : IArdalisSpecificationRepositoryFactory
{
	private readonly IServiceProvider _serviceProvider;

	public ArdalisSpecificationRepositoryFactory(IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider;
	}

	public IRepositoryBase<T> GetRepository<T>() where T : class
	{
		var serviceType = typeof(IRepositoryBase<T>);
		return (IRepositoryBase<T>)_serviceProvider.GetService(serviceType)!
		       ?? throw new InvalidOperationException($"Service for type {typeof(T).Name} not found.");
	}

	public object GetRepository(Type genericRepositoryTypeDefinition, string entityName)
	{
		var entityType = AppDomain.CurrentDomain.GetAssemblies()
			.SelectMany(assembly => assembly.GetTypes())
			.FirstOrDefault(type => type.Name.Equals(entityName, StringComparison.OrdinalIgnoreCase));

		return GetRepositoryInternally(genericRepositoryTypeDefinition, entityType, entityName);
	}

	public object GetRepository(Type genericRepositoryTypeDefinition, string entityName, string entitiesAssembly)
	{
		var assembly = AppDomain.CurrentDomain.GetAssemblies()
			.FirstOrDefault(a => a.GetName().Name == entitiesAssembly);
		var entityType = assembly?.GetTypes()
			.FirstOrDefault(t => t.Name.Equals(entityName, StringComparison.OrdinalIgnoreCase));

		return GetRepositoryInternally(genericRepositoryTypeDefinition, entityType, entityName);
	}

	public IRepositoryBase<object> GetRepository(Type type)
	{
		var repoType = typeof(IRepositoryBase<>).MakeGenericType(type);
		var service = _serviceProvider.GetService(repoType);
		if (service == null)
		{
			throw new InvalidOperationException($"Service for type {type.Name} not found.");
		}

		return (IRepositoryBase<object>)service;
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
