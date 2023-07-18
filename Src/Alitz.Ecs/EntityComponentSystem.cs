using System;
using System.Collections.Generic;

using Alitz.Collections;
using Alitz.Systems;

namespace Alitz;
public partial class EntityComponentSystem : ISystemContext
{
    private EntityComponentSystem(EntityComponentSystemOptions options, Schedule schedule)
    {
        _columnTable = options.ColumnTableFactory();
        _columnFactory = options.ColumnFactory;
        _entityPool = new IdPool();
        _schedule = schedule;
    }

    private readonly IColumnFactory _columnFactory;
    private readonly IDictionary<Type, IColumn> _columnTable;
    private readonly IdPool _entityPool;
    private readonly Schedule _schedule;

    public static Builder CreateBuilder() =>
        new();

    public void Update(long elapsedMilliseconds) =>
        _schedule.Update(this, elapsedMilliseconds);
}
