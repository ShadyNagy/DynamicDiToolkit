using System;
using Ardalis.Specification;

namespace DynamicDiToolkit.Interfaces;

public interface IArdalisSpecificationRepositoryFactory
{
	IRepositoryBase<T> GetRepository<T>() where T : class;
	IRepositoryBase<object> GetRepository(string className, string? namespaceName = null);
	IRepositoryBase<object> GetRepository(string entityName, string entitiesAssembly, string? namespaceName = null);
	IRepositoryBase<object> GetRepository(Type type);
}
