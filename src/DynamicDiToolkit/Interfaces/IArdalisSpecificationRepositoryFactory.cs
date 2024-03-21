using System;
using Ardalis.Specification;

namespace DynamicDiToolkit.Interfaces;

public interface IArdalisSpecificationRepositoryFactory
{
	IRepositoryBase<T> GetRepository<T>() where T : class;
	object GetRepository(Type genericRepositoryTypeDefinition, string className);
	object GetRepository(Type genericRepositoryTypeDefinition, string entityName, string entitiesAssembly);
	IRepositoryBase<object> GetRepository(Type type);
}
