using System;
using System.Collections.Generic;

using Alitz.Common.Collections;

namespace Alitz.EntityComponentSystem;
internal class Table
{
    public Table(IdPool entityPool, IDictionary<Type, IColumn> columns)
    {
        _entityPool = entityPool;
        _columns = columns;
    }

    private readonly IDictionary<Type, IColumn> _columns;
    private readonly IdPool _entityPool;

    public Column<TComponent> Column<TComponent>() where TComponent : struct
    {
        var componentType = typeof(TComponent);
        if (!_columns.ContainsKey(componentType))
        {
            var column = new Column<TComponent>(_entityPool);
            _columns.Add(componentType, column);
        }
        return (Column<TComponent>)_columns[componentType];
    }
}