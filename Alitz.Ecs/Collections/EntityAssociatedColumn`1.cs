using System;
using System.Collections.Generic;

namespace Alitz.Collections;
public class EntityAssociatedColumn<TComponent> : IColumn<TComponent> where TComponent : struct
{
    public EntityAssociatedColumn(IColumn<TComponent> column, IEntityManager entityEntityManager)
    {
        _column = column;
        _entityManager = entityEntityManager;
    }

    private readonly IColumn<TComponent> _column;
    private readonly IEntityManager _entityManager;

    public Type ComponentType =>
        _column.ComponentType;

    public int Count =>
        _column.Count;

    public IEnumerable<Entity> Entities =>
        _column.Entities;

    public IEnumerable<TComponent> Components =>
        _column.Components;

    public TComponent this[Entity entity]
    {
        get
        {
            ThrowIfDoesNotExist(entity);
            return _column[entity];
        }
        set
        {
            ThrowIfDoesNotExist(entity);
            _column[entity] = value;
        }
    }

    public bool TryAdd(Entity entity, TComponent component)
    {
        ThrowIfDoesNotExist(entity);
        return _column.TryAdd(entity, component);
    }

    public bool Contains(Entity entity)
    {
        ThrowIfDoesNotExist(entity);
        return _column.Contains(entity);
    }

    public bool Remove(Entity entity)
    {
        ThrowIfDoesNotExist(entity);
        return _column.Remove(entity);
    }

    public void Clear() =>
        _column.Clear();

    public bool TryGet(Entity entity, out TComponent component)
    {
        ThrowIfDoesNotExist(entity);
        return _column.TryGet(entity, out component);
    }

    public bool TrySet(Entity entity, TComponent component)
    {
        ThrowIfDoesNotExist(entity);
        return _column.TrySet(entity, component);
    }

    public ref TComponent GetByRef(Entity entity)
    {
        ThrowIfDoesNotExist(entity);
        return ref _column.GetByRef(entity);
    }

    private void ThrowIfDoesNotExist(Entity entity)
    {
        if (!_entityManager.Exists(entity))
        {
            throw new InvalidOperationException($"Entity {entity} does not exist");
        }
    }
}
