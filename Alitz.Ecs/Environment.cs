using System;
using System.Collections.Generic;

using Alitz.Collections;

namespace Alitz;
public class Environment : IEnvironment
{
    private readonly IDictionary<Type, IColumn> _columns = new Dictionary<Type, IColumn>();

    public IEntityManager EntityManager { get; } = new EntityManager(new EntityPool());

    public IColumn<TComponent> Components<TComponent>() where TComponent : struct
    {
        var componentType = typeof(TComponent);
        if (!_columns.ContainsKey(componentType))
        {
            var column = new EntityAssociatedColumn<TComponent>(new SparseColumn<TComponent>(), EntityManager);
            _columns.Add(componentType, column);
        }
        return (IColumn<TComponent>)_columns[componentType];
    }
}
