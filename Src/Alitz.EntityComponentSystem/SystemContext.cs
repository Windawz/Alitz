using System;
using System.Collections.Generic;

using Alitz.Common.Collections;

namespace Alitz.EntityComponentSystem;
public class SystemContext : ISystemContext
{
    public SystemContext(IdPool entityPool, IDictionary<Type, IColumn> columns)
    {
        EntityPool = entityPool;
        _columns = columns;
    }

    private readonly IDictionary<Type, IColumn> _columns;

    public IdPool EntityPool { get; set; }

    public Column<TComponent> Components<TComponent>() where TComponent : struct
    {
        var componentType = typeof(TComponent);
        if (!_columns.ContainsKey(componentType))
        {
            var column = new Column<TComponent>(EntityPool);
            _columns.Add(componentType, column);
        }
        return (Column<TComponent>)_columns[componentType];
    }
}