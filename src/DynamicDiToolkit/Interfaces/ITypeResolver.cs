using System;

namespace DynamicDiToolkit.Interfaces;

/// <summary>
/// Defines the interface for a type resolver.
/// </summary>
public interface ITypeResolver
{
	/// <summary>
	/// Retrieves a type by its name and optional namespace across all loaded assemblies.
	/// </summary>
	/// <param name="typeName">The name of the type.</param>
	/// <param name="namespaceName">The optional namespace of the type.</param>
	/// <returns>The resolved type, or null if not found.</returns>
	Type? GetTypeByName(string typeName, string? namespaceName = null);

	/// <summary>
	/// Retrieves a type by its name, optional namespace, and assembly name.
	/// </summary>
	/// <param name="typeName">The name of the type.</param>
	/// <param name="assemblyName">The name of the assembly containing the type.</param>
	/// <param name="namespaceName">The optional namespace of the type.</param>
	/// <returns>The resolved type, or null if not found.</returns>
	Type? GetTypeByName(string typeName, string assemblyName, string? namespaceName = null);

	/// <summary>
	/// Resolves a type instance using its full name.
	/// </summary>
	/// <param name="fullName">The full name of the type (including namespace).</param>
	/// <returns>The resolved type, or null if not found.</returns>
	Type? GetTypeByFullName(string fullName);
}
