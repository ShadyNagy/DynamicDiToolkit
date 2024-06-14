using System;
using DynamicDiToolkit.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DynamicDiToolkit.Services;

/// <summary>
/// Provides generic methods for adding, updating, deleting, and retrieving entities by ID using a dynamic approach.
/// </summary>
/// <typeparam name="TContext">The type of the DbContext.</typeparam>
public class EntityService<TContext> where TContext : DbContext
{
	private readonly TContext _context;

	/// <summary>
	/// Initializes a new instance of the <see cref="EntityService{TContext}"/> class.
	/// </summary>
	/// <param name="context">The DbContext instance.</param>
	public EntityService(TContext context)
	{
		_context = context;
	}

	/// <summary>
	/// Gets the entity type by table name and schema.
	/// </summary>
	/// <param name="tableName">The name of the table.</param>
	/// <param name="schema">The schema of the table.</param>
	/// <returns>The entity type if found; otherwise, null.</returns>
	public Type? GetEntityType(string tableName, string? schema = null)
	{
		return _context.GetEntityTypeByTableNameAndSchema(tableName, schema);
	}

	/// <summary>
	/// Creates an instance of the specified entity type.
	/// </summary>
	/// <param name="entityType">The type of the entity.</param>
	/// <returns>An instance of the specified entity type.</returns>
	public object? CreateEntity(Type entityType)
	{
		return Activator.CreateInstance(entityType);
	}

	/// <summary>
	/// Sets the property value of the specified entity.
	/// </summary>
	/// <param name="entity">The entity instance.</param>
	/// <param name="propertyName">The name of the property.</param>
	/// <param name="value">The value to set.</param>
	public void SetPropertyValue(object entity, string propertyName, object value)
	{
		var property = entity.GetType().GetProperty(propertyName);
		if (property != null && property.CanWrite)
		{
			property.SetValue(entity, value);
		}
	}

	/// <summary>
	/// Adds a new entity to the database.
	/// </summary>
	/// <param name="tableName">The name of the table.</param>
	/// <param name="schema">The schema of the table.</param>
	/// <param name="values">The values to set on the entity.</param>
	public void AddEntity(string tableName, string schema, object values)
	{
		var entityType = GetEntityType(tableName, schema);
		if (entityType == null)
		{
			throw new Exception("Entity type not found.");
		}

		var entity = CreateEntity(entityType);
		if (entity == null)
		{
			return;
		}

		var properties = values.GetType().GetProperties();
		foreach (var property in properties)
		{
			var propertyName = property.Name;
			var value = property.GetValue(values);
			if (value == null)
			{
				continue;
			}
			SetPropertyValue(entity, propertyName, value);
		}

		_context.Add(entity);
		_context.SaveChanges();
	}

	/// <summary>
	/// Updates an existing entity in the database.
	/// </summary>
	/// <param name="tableName">The name of the table.</param>
	/// <param name="schema">The schema of the table.</param>
	/// <param name="id">The ID of the entity to update.</param>
	/// <param name="values">The values to set on the entity.</param>
	public void UpdateEntity(string tableName, string schema, object id, object values)
	{
		var entityType = GetEntityType(tableName, schema);
		if (entityType == null)
		{
			throw new Exception("Entity type not found.");
		}

		var entity = _context.Find(entityType, id);
		if (entity == null)
		{
			throw new Exception("Entity not found.");
		}

		var properties = values.GetType().GetProperties();
		foreach (var property in properties)
		{
			var propertyName = property.Name;
			var value = property.GetValue(values);
			if (value == null)
			{
				continue;
			}
			SetPropertyValue(entity, propertyName, value);
		}

		_context.Update(entity);
		_context.SaveChanges();
	}

	/// <summary>
	/// Deletes an entity from the database.
	/// </summary>
	/// <param name="tableName">The name of the table.</param>
	/// <param name="schema">The schema of the table.</param>
	/// <param name="id">The ID of the entity to delete.</param>
	public void DeleteEntity(string tableName, string schema, object id)
	{
		var entityType = GetEntityType(tableName, schema);
		if (entityType == null)
		{
			throw new Exception("Entity type not found.");
		}

		var entity = _context.Find(entityType, id);
		if (entity == null)
		{
			throw new Exception("Entity not found.");
		}

		_context.Remove(entity);
		_context.SaveChanges();
	}

	/// <summary>
	/// Gets an entity by its ID.
	/// </summary>
	/// <param name="tableName">The name of the table.</param>
	/// <param name="schema">The schema of the table.</param>
	/// <param name="id">The ID of the entity to retrieve.</param>
	/// <returns>The entity if found; otherwise, null.</returns>
	public object? GetEntityById(string tableName, string schema, object id)
	{
		var entityType = GetEntityType(tableName, schema);
		if (entityType == null)
		{
			throw new Exception("Entity type not found.");
		}

		return _context.Find(entityType, id);
	}
}
