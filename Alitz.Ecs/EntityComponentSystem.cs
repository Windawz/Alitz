﻿using System;
using System.Collections.Generic;

using Alitz.Collections;
using Alitz.Systems;

namespace Alitz;
public partial class EntityComponentSystem : ISystemContext
{
    public EntityComponentSystem(EntityComponentSystemOptions options, Schedule schedule)
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

    public void Update(double delta) =>
        _schedule.Update(this, delta);
}
