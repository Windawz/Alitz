using System;
using System.Collections.Generic;

using Alitz.Collections;
using Alitz.Systems;

namespace Alitz;
public partial class EntityComponentSystem : IEnvironment
{
    public EntityComponentSystem(EntityComponentSystemOptions options, Schedule schedule)
    {
        _columnTable = options.ColumnTableFactory();
        _entityManager = options.EntityManagerFactory(options.EntityPoolFactory());
        _schedule = schedule;
    }

    private readonly IDictionary<Type, IColumn> _columnTable;
    private readonly IEntityManager _entityManager;
    private readonly Schedule _schedule;

    public void Update(double delta) =>
        _schedule.Update(this, delta);
}
