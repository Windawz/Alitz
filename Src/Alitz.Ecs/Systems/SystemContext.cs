using System;
using System.Collections.Generic;

using Alitz.Collections;

namespace Alitz.Systems;
public class SystemContext
{
    public SystemContext(IDictionary<Type, IColumn> columnTable, IPool<Id> entityPool)
    {
        _columnTable = columnTable;
        EntityPool = entityPool;
    }

    private readonly IDictionary<Type, IColumn> _columnTable;

    public IPool<Id> EntityPool { get; }

    public IColumn<TComponent> Components<TComponent>() where TComponent : struct
    {
        var componentType = typeof(TComponent);
        if (!_columnTable.ContainsKey(componentType))
        {
            var column = new EntityAssociatedColumn<TComponent>(new SparseColumn<TComponent>(), EntityPool);
            _columnTable.Add(componentType, column);
        }
        return (IColumn<TComponent>)_columnTable[componentType];
    }
}
