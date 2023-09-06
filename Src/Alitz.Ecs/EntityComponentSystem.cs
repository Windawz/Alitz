using System;
using System.Collections.Generic;

using Alitz.Collections;
using Alitz.Systems;

namespace Alitz;
public class EntityComponentSystem : ISystemContext
{
    internal EntityComponentSystem(SystemSchedule systemSchedule)
    {
        _columnTable = new Dictionary<Type, IColumn>();
        _systemSchedule = systemSchedule;
        EntityPool = new IdPool();
    }

    private readonly IDictionary<Type, IColumn> _columnTable;
    private readonly SystemSchedule _systemSchedule;

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

    public static EcsBuilder CreateBuilder() =>
        new();

    public void Update(long deltaMs) =>
        _systemSchedule.Update(this, deltaMs);
}
