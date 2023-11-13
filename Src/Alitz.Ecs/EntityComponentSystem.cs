using System;
using System.Collections.Generic;
using System.Linq;

using Alitz.Common.Collections;



namespace Alitz.Ecs;
public class EntityComponentSystem : ISystemContext
{
    public EntityComponentSystem(IReadOnlyCollection<ISystem> systems)
    {
        _columns = new Dictionary<Type, IColumn>();
        _systems = systems.ToArray();
        EntityPool = new IdPool();
    }

    private readonly IDictionary<Type, IColumn> _columns;
    private readonly IReadOnlyCollection<ISystem> _systems;

    public IdPool EntityPool { get; }

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

    public void Update(long deltaMs)
    {
        foreach (var system in _systems)
        {
            system.Update(this, deltaMs);
        }
    }
}
