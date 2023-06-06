using System.Collections.Generic;

namespace Alitz.Ecs.Collections;
public class EntitySpace
{
    private readonly StackSparseSet<Entity> _recycledEntities = new(IndexExtractor.Entity);
    private readonly StackSparseSet<Entity> _takenEntities = new(IndexExtractor.Entity);

    public int Count =>
        _takenEntities.Count;

    public IEnumerable<Entity> Entities =>
        _takenEntities.Values;

    public Entity Create()
    {
        Entity entity;
        if (_recycledEntities.Count > 0)
        {
            _ = _recycledEntities.TryPop(out var recycledEntity);
            entity = new Entity(recycledEntity.Id, recycledEntity.Version + 1);
        }
        else if (_takenEntities.Count > 0)
        {
            _ = _takenEntities.TryPeek(out var takenEntity);
            int id = takenEntity.Id + 1;
            entity = new Entity(id);
        }
        else
        {
            entity = new Entity(0);
        }
        _takenEntities.Add(entity);
        return entity;
    }

    public bool Exists(Entity entity) =>
        _takenEntities.Contains(entity);

    public bool Destroy(Entity entity)
    {
        if (_takenEntities.Remove(entity))
        {
            _recycledEntities.Add(entity);
            return true;
        }
        return false;
    }
}
