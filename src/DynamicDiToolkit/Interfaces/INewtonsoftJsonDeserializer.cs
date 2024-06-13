using System;
using Newtonsoft.Json;

namespace DynamicDiToolkit.Interfaces;

/// <summary>
/// Defines the interface for a JSON deserializer.
/// </summary>
public interface INewtonsoftJsonDeserializer
{
	T Deserialize<T>(string json, JsonSerializerSettings? settings = null);
	object Deserialize(string json, Type type, JsonSerializerSettings? settings = null);
	object Deserialize(string json, string typeName, string? namespaceName = null, JsonSerializerSettings? settings = null);
	object Deserialize(string json, string typeName, string assemblyName, string? namespaceName = null, JsonSerializerSettings? settings = null);
}
