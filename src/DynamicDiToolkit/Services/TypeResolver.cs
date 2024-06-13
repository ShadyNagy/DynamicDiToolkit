using System;
using System.Linq;
using DynamicDiToolkit.Interfaces;

namespace DynamicDiToolkit.Services;

/// <summary>
/// Represents a utility for retrieving types by their name, namespace, and assembly.
/// This utility helps in dynamically finding and resolving types, which can be useful
/// for various dependency injection and reflection-based scenarios.
/// </summary>
public class TypeResolver : ITypeResolver
{
	/// <summary>
	/// Retrieves a type by its name and optional namespace across all loaded assemblies.
	/// </summary>
	/// <param name="typeName">The name of the type.</param>
	/// <param name="namespaceName">The optional namespace of the type.</param>
	/// <returns>The resolved type, or null if not found.</returns>
	public Type? GetTypeByName(string typeName, string? namespaceName = null)
	{
		var type = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(assembly => assembly.GetTypes())
				.FirstOrDefault(t => t.Name.Equals(typeName, StringComparison.OrdinalIgnoreCase) &&
														 (namespaceName == null || (t.Namespace != null && t.Namespace.Equals(namespaceName, StringComparison.OrdinalIgnoreCase))));

		return type;
	}

	/// <summary>
	/// Retrieves a type by its name, optional namespace, and assembly name.
	/// </summary>
	/// <param name="typeName">The name of the type.</param>
	/// <param name="assemblyName">The name of the assembly containing the type.</param>
	/// <param name="namespaceName">The optional namespace of the type.</param>
	/// <returns>The resolved type, or null if not found.</returns>
	public Type? GetTypeByName(string typeName, string assemblyName, string? namespaceName = null)
	{
		var assembly = AppDomain.CurrentDomain.GetAssemblies()
				.FirstOrDefault(a => a?.GetName()?.Name != null && a.GetName().Name!.Equals(assemblyName, StringComparison.OrdinalIgnoreCase));

		if (assembly == null)
		{
			return null;
		}

		var type = assembly.GetTypes()
				.FirstOrDefault(t => t.Name.Equals(typeName, StringComparison.OrdinalIgnoreCase) &&
														 (namespaceName == null || (t.Namespace != null && t.Namespace.Equals(namespaceName, StringComparison.OrdinalIgnoreCase))));

		return type;
	}

	/// <summary>
	/// Resolves a type instance using its full name.
	/// </summary>
	/// <param name="fullName">The full name of the type (including namespace).</param>
	/// <returns>The resolved type, or null if not found.</returns>
	public Type? GetTypeByFullName(string fullName)
	{
		var type = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(assembly => assembly.GetTypes())
				.FirstOrDefault(t => t.FullName != null && t.FullName.Equals(fullName, StringComparison.OrdinalIgnoreCase));

		return type;
	}
}
