using System;
using System.Collections.Generic;

using Alitz.Collections;

namespace Alitz;
public class EntityComponentSystemOptions
{
    public Func<IDictionary<Type, IColumn>> ColumnTableFactory { get; init; } = () => new Dictionary<Type, IColumn>();
    public Func<IPool<Entity>> EntityPoolFactory { get; init; } = () =>
        new IdPool<Entity>(new ReflectingIdFactory<Entity>());
    public Func<IPool<Entity>, IEntityManager> EntityManagerFactory { get; init; } = pool => new EntityManager(pool);
}
