﻿using Alitz.Collections;

namespace Alitz;
public partial class EntityComponentSystem
{
    IPool<Entity> IEnvironment.EntityPool =>
        _entityPool;

    IColumn<TComponent> IEnvironment.Components<TComponent>() where TComponent : struct
    {
        var componentType = typeof(TComponent);
        if (!_columnTable.ContainsKey(componentType))
        {
            var column = new EntityAssociatedColumn<TComponent>(_columnFactory.Create<TComponent>(), _entityPool);
            _columnTable.Add(componentType, column);
        }
        return (IColumn<TComponent>)_columnTable[componentType];
    }
}
