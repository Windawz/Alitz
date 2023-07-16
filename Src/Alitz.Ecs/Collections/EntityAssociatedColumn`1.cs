using System;
using System.Collections.Generic;

namespace Alitz.Collections;
public class EntityAssociatedColumn<TComponent> : IColumn<TComponent> where TComponent : struct
{
    public EntityAssociatedColumn(IColumn<TComponent> column, IPool<Id> entityPool)
    {
        _column = column;
        _entityPool = entityPool;
    }

    private readonly IColumn<TComponent> _column;
    private readonly IPool<Id> _entityPool;

    public Type ComponentType =>
        _column.ComponentType;

    public int Count =>
        _column.Count;

    public IEnumerable<Id> Entities =>
        _column.Entities;

    public IEnumerable<TComponent> Components =>
        _column.Components;

    public TComponent this[Id entity]
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

    public bool TryAdd(Id entity, TComponent component)
    {
        ThrowIfDoesNotExist(entity);
        return _column.TryAdd(entity, component);
    }

    public bool Contains(Id entity)
    {
        ThrowIfDoesNotExist(entity);
        return _column.Contains(entity);
    }

    public bool Remove(Id entity)
    {
        ThrowIfDoesNotExist(entity);
        return _column.Remove(entity);
    }

    public void Clear() =>
        _column.Clear();

    public bool TryGet(Id entity, out TComponent component)
    {
        ThrowIfDoesNotExist(entity);
        return _column.TryGet(entity, out component);
    }

    public bool TrySet(Id entity, TComponent component)
    {
        ThrowIfDoesNotExist(entity);
        return _column.TrySet(entity, component);
    }

    public ref TComponent GetByRef(Id entity)
    {
        ThrowIfDoesNotExist(entity);
        return ref _column.GetByRef(entity);
    }

    private void ThrowIfDoesNotExist(Id entity)
    {
        if (!_entityPool.IsOccupied(entity))
        {
            throw new InvalidOperationException($"Id {entity} does not exist");
        }
    }
}
