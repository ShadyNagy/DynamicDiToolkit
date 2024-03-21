using System;
using Ardalis.Specification;

namespace DynamicDiToolkit.Interfaces;

public interface IRepositoryFactory
{
	T GetRepository<T>() where T : class;
	object GetRepository(Type genericRepositoryTypeDefinition, string className);
	object GetRepository(Type genericRepositoryTypeDefinition, string entityName, string entitiesAssembly);
	object GetRepository(Type type);
}
