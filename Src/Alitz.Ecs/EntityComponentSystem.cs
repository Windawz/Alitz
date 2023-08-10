using System;
using System.Collections.Generic;

using Alitz.Collections;
using Alitz.Systems;

namespace Alitz;
public partial class EntityComponentSystem
{
    private EntityComponentSystem(Schedule schedule)
    {
        _columnTable = new Dictionary<Type, IColumn>();
        _entityPool = new IdPool();
        _schedule = schedule;
        _systemContext = new SystemContext(_columnTable, _entityPool);
    }

    private readonly IDictionary<Type, IColumn> _columnTable;
    private readonly IdPool _entityPool;
    private readonly Schedule _schedule;
    private readonly SystemContext _systemContext;

    public static Builder CreateBuilder() =>
        new();

    public void Update(long elapsedMilliseconds) =>
        _schedule.Update(_systemContext, elapsedMilliseconds);
}
