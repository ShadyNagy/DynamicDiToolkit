using System;
using System.Linq;
using System.Text.Json;
using DynamicDiToolkit.Interfaces;

namespace DynamicDiToolkit.Services;

/// <summary>
/// Provides functionality for deserializing JSON using Microsoft's System.Text.Json.
/// </summary>
public class SystemTextJsonDeserializer : ISystemTextJsonDeserializer
{
	/// <summary>
	/// Deserializes the JSON string to an instance of the specified type.
	/// </summary>
	/// <typeparam name="T">The type to which the JSON should be deserialized.</typeparam>
	/// <param name="json">The JSON string to deserialize.</param>
	/// <param name="options">Optional JsonSerializerOptions to use during deserialization.</param>
	/// <returns>An instance of the specified type.</returns>
	/// <exception cref="JsonException">Thrown if the JSON is invalid or cannot be deserialized to the specified type.</exception>
	public T Deserialize<T>(string json, JsonSerializerOptions? options = null)
	{
		return JsonSerializer.Deserialize<T>(json, options) ?? throw new JsonException($"Deserialization to type {typeof(T).Name} failed.");
	}

	/// <summary>
	/// Deserializes the JSON string to an instance of the specified type.
	/// </summary>
	/// <param name="json">The JSON string to deserialize.</param>
	/// <param name="type">The type to which the JSON should be deserialized.</param>
	/// <param name="options">Optional JsonSerializerOptions to use during deserialization.</param>
	/// <returns>An instance of the specified type.</returns>
	/// <exception cref="JsonException">Thrown if the JSON is invalid or cannot be deserialized to the specified type.</exception>
	public object Deserialize(string json, Type type, JsonSerializerOptions? options = null)
	{
		return JsonSerializer.Deserialize(json, type, options) ?? throw new JsonException($"Deserialization to type {type.Name} failed.");
	}

	/// <summary>
	/// Deserializes the JSON string to an instance of the specified type, resolving the type by name and optional namespace.
	/// </summary>
	/// <param name="json">The JSON string to deserialize.</param>
	/// <param name="typeName">The name of the type.</param>
	/// <param name="namespaceName">The optional namespace of the type.</param>
	/// <param name="options">Optional JsonSerializerOptions to use during deserialization.</param>
	/// <returns>An instance of the resolved type.</returns>
	/// <exception cref="InvalidOperationException">Thrown if the type cannot be resolved.</exception>
	public object Deserialize(string json, string typeName, string? namespaceName = null, JsonSerializerOptions? options = null)
	{
		var type = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(assembly => assembly.GetTypes())
				.FirstOrDefault(t => t.Name.Equals(typeName, StringComparison.OrdinalIgnoreCase) &&
														 (namespaceName == null || (t.Namespace != null && t.Namespace.Equals(namespaceName, StringComparison.OrdinalIgnoreCase))));

		if (type == null)
		{
			throw new InvalidOperationException($"Type {typeName} not found.");
		}

		return Deserialize(json, type, options);
	}

	/// <summary>
	/// Deserializes the JSON string to an instance of the specified type, resolving the type by name, namespace, and assembly.
	/// </summary>
	/// <param name="json">The JSON string to deserialize.</param>
	/// <param name="typeName">The name of the type.</param>
	/// <param name="assemblyName">The name of the assembly containing the type.</param>
	/// <param name="namespaceName">The optional namespace of the type.</param>
	/// <param name="options">Optional JsonSerializerOptions to use during deserialization.</param>
	/// <returns>An instance of the resolved type.</returns>
	/// <exception cref="InvalidOperationException">Thrown if the type cannot be resolved.</exception>
	public object Deserialize(string json, string typeName, string assemblyName, string? namespaceName = null, JsonSerializerOptions? options = null)
	{
		var assembly = AppDomain.CurrentDomain.GetAssemblies()
				.FirstOrDefault(a => a?.GetName()?.Name != null && a.GetName().Name!.Equals(assemblyName, StringComparison.OrdinalIgnoreCase));

		if (assembly == null)
		{
			throw new InvalidOperationException($"Assembly {assemblyName} not found.");
		}

		var type = assembly.GetTypes()
				.FirstOrDefault(t => t.Name.Equals(typeName, StringComparison.OrdinalIgnoreCase) &&
														 (namespaceName == null || (t.Namespace != null && t.Namespace.Equals(namespaceName, StringComparison.OrdinalIgnoreCase))));

		if (type == null)
		{
			throw new InvalidOperationException($"Type {typeName} not found in assembly {assemblyName}.");
		}

		return Deserialize(json, type, options);
	}
}
