namespace Alitz.Ecs.Collections;
public class EntityPool
{
    private readonly StackSparseSet<Entity> _recycledEntities = new(IndexExtractors.EntityIndexExtractor);
    private readonly StackSparseSet<Entity> _takenEntities = new(IndexExtractors.EntityIndexExtractor);

    public int TakenCount =>
        _takenEntities.Count;

    public Entity Take()
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

    public bool Taken(Entity entity) =>
        _takenEntities.Contains(entity);

    public bool Recycle(Entity entity)
    {
        if (_takenEntities.Remove(entity))
        {
            _recycledEntities.Add(entity);
            return true;
        }
        return false;
    }

    private class StackSparseSet<T> : SparseSet<T>
    {
        public StackSparseSet(IndexExtractor<T> indexExtractor) : base(indexExtractor) { }

        public bool TryPeek(out T? value)
        {
            if (Count > 0)
            {
                value = Dense[^1];
                return true;
            }
            value = default;
            return false;
        }

        public bool TryPop(out T? value)
        {
            if (Count == 0)
            {
                value = default;
                return false;
            }
            value = Dense[^1];
            Remove(value);
            return true;
        }
    }
}
