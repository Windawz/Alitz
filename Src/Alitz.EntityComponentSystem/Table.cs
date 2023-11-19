using System;
using System.Collections.Generic;

using Alitz.Common.Collections;

namespace Alitz.EntityComponentSystem;
public class Table : ITable
{
    public Table(IdPool entityPool)
    {
        _entityPool = entityPool;
    }

    private readonly Dictionary<Type, IColumn> _columns = new();
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