using System;
using System.Collections.Generic;

using Alitz.Collections;
using Alitz.Systems;

namespace Alitz;
public class EntityComponentSystem
{
    internal EntityComponentSystem(SystemSchedule systemSchedule)
    {
        var columnTable = new Dictionary<Type, IColumn>();
        var entityPool = new IdPool();
        _systemSchedule = systemSchedule;
        _systemContext = new SystemContext(columnTable, entityPool);
    }

    private readonly SystemContext _systemContext;
    private readonly SystemSchedule _systemSchedule;

    public void Update(long elapsedMilliseconds) =>
        _systemSchedule.Update(_systemContext, elapsedMilliseconds);
}
