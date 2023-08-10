using System;
using System.Collections.Generic;

namespace Alitz.Collections;
public class Column<TComponent> : IColumn where TComponent : struct
{
    private const int DenseInitialCapacity = 4;
    private const int SparseFillValue = -1;
    private const int SparseInitialCapacity = 4;

    public Column(IPool<Id> entityPool)
    {
        Array.Fill(_sparse, SparseFillValue);
        _entityPool = entityPool;
    }

    private readonly IPool<Id> _entityPool;

    private TComponent[] _denseComponents = new TComponent[DenseInitialCapacity];
    private Id[] _denseEntities = new Id[DenseInitialCapacity];
    private int[] _sparse = new int[SparseInitialCapacity];

    public TComponent this[Id entity]
    {
        get
        {
            ThrowIfEntityDoesNotExist(entity);
            if (!Contains(entity))
            {
                throw new ArgumentOutOfRangeException(nameof(entity));
            }
            return _denseComponents[_sparse[entity.Index]];
        }
        set
        {
            ThrowIfEntityDoesNotExist(entity);
            if (!Contains(entity))
            {
                TryAdd(entity, value);
            }
            else
            {
                _denseComponents[_sparse[entity.Index]] = value;
            }
        }
    }

    public IEnumerable<TComponent> Components
    {
        get
        {
            for (int i = 0; i < Count; i++)
            {
                yield return _denseComponents[i];
            }
        }
    }

    Type IColumn.ComponentType =>
        typeof(TComponent);

    public int Count { get; private set; }

    public IEnumerable<Id> Entities
    {
        get
        {
            for (int i = 0; i < Count; i++)
            {
                yield return _denseEntities[i];
            }
        }
    }

    public bool Contains(Id entity)
    {
        ThrowIfEntityDoesNotExist(entity);
        return entity.Index < _sparse.Length
            && _sparse[entity.Index] != SparseFillValue
            && _denseEntities[_sparse[entity.Index]].Equals(entity);
    }

    public bool Remove(Id entity)
    {
        ThrowIfEntityDoesNotExist(entity);
        if (!Contains(entity))
        {
            return false;
        }
        Count -= 1;
        int denseIndex = _sparse[entity.Index];
        _sparse[entity.Index] = SparseFillValue;
        _denseEntities[denseIndex] = _denseEntities[Count];
        _denseComponents[denseIndex] = _denseComponents[Count];
        return true;
    }

    public void Clear()
    {
        Array.Clear(_sparse);
        Array.Clear(_denseEntities);
        Array.Clear(_denseComponents);
        Count = 0;
    }

    public bool TryAdd(Id entity, TComponent component)
    {
        ThrowIfEntityDoesNotExist(entity);
        if (Contains(entity))
        {
            return false;
        }
        Count += 1;
        Grow(ref _sparse, entity.Index + 1);
        Grow(ref _denseEntities, Count);
        Grow(ref _denseComponents, Count);
        _sparse[entity.Index] = Count - 1;
        _denseEntities[Count - 1] = entity;
        _denseComponents[Count - 1] = component;
        return true;
    }

    public bool TryGet(Id entity, out TComponent component)
    {
        ThrowIfEntityDoesNotExist(entity);
        if (!Contains(entity))
        {
            component = default!;
            return false;
        }
        component = _denseComponents[_sparse[entity.Index]];
        return true;
    }

    public bool TrySet(Id entity, TComponent component)
    {
        ThrowIfEntityDoesNotExist(entity);
        if (!Contains(entity))
        {
            return false;
        }
        _denseComponents[_sparse[entity.Index]] = component;
        return true;
    }

    public ref TComponent GetByRef(Id entity)
    {
        ThrowIfEntityDoesNotExist(entity);
        if (!Contains(entity))
        {
            throw new ArgumentOutOfRangeException(nameof(entity));
        }
        return ref _denseComponents[_sparse[entity.Index]];
    }

    private void ThrowIfEntityDoesNotExist(Id entity)
    {
        if (!_entityPool.IsOccupied(entity))
        {
            throw new InvalidOperationException($"Entity with id {entity} does not exist");
        }
    }

    private static void Grow<T>(ref T[] array, int length)
    {
        if (array.Length >= length)
        {
            return;
        }
        int newLength = array.Length;
        while (newLength < length)
        {
            newLength *= 2;
        }
        Array.Resize(ref array, newLength);
    }
}
