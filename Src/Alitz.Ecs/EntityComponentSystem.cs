using System;
using System.Collections.Generic;

using Alitz.Collections;
using Alitz.Systems;

namespace Alitz;
public partial class EntityComponentSystem
{
    private EntityComponentSystem(SystemSchedule systemSchedule)
    {
        _columnTable = new Dictionary<Type, IColumn>();
        _entityPool = new IdPool();
        _systemSchedule = systemSchedule;
        _systemContext = new SystemContext(_columnTable, _entityPool);
    }

    private readonly IDictionary<Type, IColumn> _columnTable;
    private readonly IdPool _entityPool;
    private readonly SystemContext _systemContext;
    private readonly SystemSchedule _systemSchedule;

    public static Builder CreateBuilder() =>
        new();

    public void Update(long elapsedMilliseconds) =>
        _systemSchedule.Update(_systemContext, elapsedMilliseconds);
}
