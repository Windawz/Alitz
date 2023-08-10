using System;
using System.Collections.Generic;

using Alitz.Collections;

namespace Alitz.Systems;
public class SystemContext
{
    public SystemContext(IDictionary<Type, IColumn> columnTable, IdPool entityPool)
    {
        _columnTable = columnTable;
        EntityPool = entityPool;
    }

    private readonly IDictionary<Type, IColumn> _columnTable;

    public IdPool EntityPool { get; }

    public Column<TComponent> Components<TComponent>() where TComponent : struct
    {
        var componentType = typeof(TComponent);
        if (!_columnTable.ContainsKey(componentType))
        {
            var column = new Column<TComponent>(EntityPool);
            _columnTable.Add(componentType, column);
        }
        return (Column<TComponent>)_columnTable[componentType];
    }
}
