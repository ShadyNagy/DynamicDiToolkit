using System;
using Microsoft.EntityFrameworkCore;

namespace DynamicDiToolkit.Extensions;

/// <summary>
/// Provides extension methods for the <see cref="DbContext"/> class.
/// </summary>
public static class DbContextExtensions
{
	/// <summary>
	/// Gets the entity type associated with the specified table name and schema.
	/// </summary>
	/// <param name="context">The <see cref="DbContext"/> instance.</param>
	/// <param name="tableName">The name of the table.</param>
	/// <param name="schema">The schema of the table. If null, defaults to 'dbo'.</param>
	/// <returns>The <see cref="Type"/> of the entity associated with the specified table name and schema, or null if not found.</returns>
	public static Type? GetEntityTypeByTableNameAndSchema(this DbContext context, string tableName, string? schema = null)
	{
		var entityTypes = context.Model.GetEntityTypes();
		foreach (var entityType in entityTypes)
		{
			var relationalData = entityType.GetAnnotation("Relational:TableName");
			var currentTableName = relationalData?.Value?.ToString();

			var currentSchema = entityType.GetAnnotation("Relational:Schema")?.Value?.ToString() ?? "dbo";

			if (string.Equals(currentTableName, tableName, StringComparison.OrdinalIgnoreCase) &&
					(string.IsNullOrEmpty(schema) || string.Equals(currentSchema, schema, StringComparison.OrdinalIgnoreCase)))
			{
				return entityType.ClrType;
			}
		}

		return null;
	}
}

