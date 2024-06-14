using System;
using Microsoft.EntityFrameworkCore;

namespace DynamicDiToolkit.Extensions;

public static class DbContextExtensions
{
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
