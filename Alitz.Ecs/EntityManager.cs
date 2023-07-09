using System.Collections.Generic;

using Alitz.Collections;

namespace Alitz;
public class EntityManager : IEntityManager
{
    public EntityManager(IPool<Entity> entityPool)
    {
        _entityPool = entityPool;
    }

    private readonly IPool<Entity> _entityPool;

    public IReadOnlyCollection<Entity> Entities =>
        _entityPool.Occupied;

    public Entity Create() =>
        _entityPool.Fetch();

    public bool Exists(Entity entity) =>
        _entityPool.IsOccupied(entity);

    public void Destroy(Entity entity) =>
        _entityPool.Store(entity);
}
