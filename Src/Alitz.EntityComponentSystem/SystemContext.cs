using System;

using Alitz.Common;
using Alitz.Common.Collections;

namespace Alitz.EntityComponentSystem;
public class SystemContext : ISystemContext, IEntitiesContext, IEntityContext
{
    public SystemContext(IdPool entityPool, ITable table)
    {
        _entityPool = entityPool;
        _table = table;
    }

    private readonly IdPool _entityPool;
    private readonly ITable _table;
    private Id? _selectedEntity = null;

    Id IEntityContext.Entity =>
        _selectedEntity!.Value;

    IEntitiesContext ISystemContext.Entities =>
        this;

    IEntityContext? ISystemContext.GetEntity(Id entity)
    {
        if (!_entityPool.IsOccupied(entity))
        {
            return null;
        }
        else
        {
            _selectedEntity = entity;
            return this;
        }
    }

    IEntityContext ISystemContext.NewEntity()
    {
        _selectedEntity = _entityPool.Fetch();
        return this;
    }

    void IEntitiesContext.RawForEach<TEntityEnumerator>(Func<IdPool, ITable, TEntityEnumerator> enumeratorFactory, Action<IEntityContext, ITable> action)
    {
        using var enumerator = enumeratorFactory(_entityPool, _table);
        while (enumerator.MoveNext())
        {
            _selectedEntity = enumerator.Current;
            action(this, _table);
        }
    }

    ref TComponent IEntityContext.Component<TComponent>() where TComponent : struct
    {
        ref var component = ref _table.Column<TComponent>().GetByRef(_selectedEntity!.Value);
        return ref component;
    }

    void IEntityContext.Destroy()
    {
        _entityPool.Store(_selectedEntity!.Value);
    }
}