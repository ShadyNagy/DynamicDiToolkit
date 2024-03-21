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

	public IRepositoryBase<object> GetRepository(string entityName)
	{
		var entityType = AppDomain.CurrentDomain.GetAssemblies()
			.SelectMany(assembly => assembly.GetTypes())
			.FirstOrDefault(type => type.Name.Equals(entityName, StringComparison.OrdinalIgnoreCase));

		return GetRepositoryInternally(entityType, entityName);
	}

	public IRepositoryBase<object> GetRepository(string entityName, string entitiesAssembly)
	{
		var assembly = AppDomain.CurrentDomain.GetAssemblies()
			.FirstOrDefault(a => a.GetName().Name == entitiesAssembly);
		var entityType = assembly?.GetTypes()
			.FirstOrDefault(t => t.Name.Equals(entityName, StringComparison.OrdinalIgnoreCase));

		return GetRepositoryInternally(entityType, entityName);
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

	private IRepositoryBase<object> GetRepositoryInternally(Type? entityType, string entityName)
	{
		if (entityType == null)
		{
			throw new InvalidOperationException($"Entity type {entityName} not found.");
		}

		var repositoryGenericType = typeof(IRepositoryBase<>).MakeGenericType(entityType);
		var repository = _serviceProvider.GetService(repositoryGenericType);

		if (repository == null)
		{
			throw new InvalidOperationException($"Repository for entity type {entityName} not found.");
		}

		return (IRepositoryBase<object>)repository;
	}
}
