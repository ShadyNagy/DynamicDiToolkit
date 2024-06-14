using System;
using DynamicDiToolkit.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DynamicDiToolkit.Services;

public class EntityService<TContext> where TContext : DbContext
{
	private readonly TContext _context;

	public EntityService(TContext context)
	{
		_context = context;
	}

	public Type? GetEntityType(string tableName, string? schema = null)
	{
		return _context.GetEntityTypeByTableNameAndSchema(tableName, schema);
	}

	public object? CreateEntity(Type entityType)
	{
		return Activator.CreateInstance(entityType);
	}

	public void SetPropertyValue(object entity, string propertyName, object value)
	{
		var property = entity.GetType().GetProperty(propertyName);
		if (property != null && property.CanWrite)
		{
			property.SetValue(entity, value);
		}
	}

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
