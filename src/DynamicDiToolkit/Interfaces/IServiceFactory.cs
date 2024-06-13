using System;

namespace DynamicDiToolkit.Interfaces;

/// <summary>
/// Defines a factory for creating instances of repositories. This interface abstracts the instantiation
/// process of service instances to promote loose coupling and ease of dependency injection.
/// </summary>
public interface IServiceFactory
{
	/// <summary>
	/// Gets an instance of a service of type <typeparamref name="T"/>.
	/// </summary>
	/// <typeparam name="T">The type of the service interface to resolve.</typeparam>
	/// <returns>An instance of the service of type <typeparamref name="T"/>.</returns>
	T GetService<T>() where T : class;

	/// <summary>
	/// Gets a service instance for the specified class using a generic service type definition.
	/// </summary>
	/// <param name="genericServiceTypeDefinition">The generic service type definition.</param>
	/// <param name="className">The name of the class.</param>
	/// <returns>A service instance for the specified class.</returns>
	object GetService(Type genericServiceTypeDefinition, string className, string? namespaceName = null);

	/// <summary>
	/// Gets a service instance for the specified entity name within a specific assembly, using a generic service type definition.
	/// </summary>
	/// <param name="genericServiceTypeDefinition">The generic service type definition.</param>
	/// <param name="entityName">The name of the entity.</param>
	/// <param name="entitiesAssembly">The name of the assembly containing the entity.</param>
	/// <returns>A service instance for the specified entity.</returns>
	object GetService(Type genericServiceTypeDefinition, string entityName, string entitiesAssembly, string? namespaceName = null);

	/// <summary>
	/// Gets a service instance for the specified type.
	/// </summary>
	/// <param name="type">The entity type for which to retrieve the service.</param>
	/// <returns>A service instance for the specified type.</returns>
	object GetService(Type type);

}
