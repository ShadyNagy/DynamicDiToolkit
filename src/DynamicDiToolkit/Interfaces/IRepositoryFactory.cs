using System;

namespace DynamicDiToolkit.Interfaces;

/// <summary>
/// Defines a factory for creating instances of repositories. This interface abstracts the instantiation
/// process of repository instances to promote loose coupling and ease of dependency injection.
/// </summary>
public interface IRepositoryFactory
{
	/// <summary>
	/// Gets an instance of a repository of type <typeparamref name="T"/>.
	/// </summary>
	/// <typeparam name="T">The type of the repository interface to resolve.</typeparam>
	/// <returns>An instance of the repository of type <typeparamref name="T"/>.</returns>
	T GetRepository<T>() where T : class;

	/// <summary>
	/// Gets a repository instance for the specified class using a generic repository type definition.
	/// </summary>
	/// <param name="genericRepositoryTypeDefinition">The generic repository type definition.</param>
	/// <param name="className">The name of the class.</param>
	/// <returns>A repository instance for the specified class.</returns>
	object GetRepository(Type genericRepositoryTypeDefinition, string className);

	/// <summary>
	/// Gets a repository instance for the specified entity name within a specific assembly, using a generic repository type definition.
	/// </summary>
	/// <param name="genericRepositoryTypeDefinition">The generic repository type definition.</param>
	/// <param name="entityName">The name of the entity.</param>
	/// <param name="entitiesAssembly">The name of the assembly containing the entity.</param>
	/// <returns>A repository instance for the specified entity.</returns>
	object GetRepository(Type genericRepositoryTypeDefinition, string entityName, string entitiesAssembly);

	/// <summary>
	/// Gets a repository instance for the specified type.
	/// </summary>
	/// <param name="type">The entity type for which to retrieve the repository.</param>
	/// <returns>A repository instance for the specified type.</returns>
	object GetRepository(Type type);
}
