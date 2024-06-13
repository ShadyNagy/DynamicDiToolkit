using System;
using System.Text.Json;

namespace DynamicDiToolkit.Interfaces;

/// <summary>
/// Defines the interface for a JSON deserializer.
/// </summary>
public interface ISystemTextJsonDeserializer
{
	T Deserialize<T>(string json, JsonSerializerOptions? options = null);
	object Deserialize(string json, Type type, JsonSerializerOptions? options = null);
	object Deserialize(string json, string typeName, string? namespaceName = null, JsonSerializerOptions? options = null);
	object Deserialize(string json, string typeName, string assemblyName, string? namespaceName = null, JsonSerializerOptions? options = null);
}
