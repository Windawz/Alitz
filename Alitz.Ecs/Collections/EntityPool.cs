namespace Alitz.Ecs.Collections;
public class EntityPool {
    private readonly EntitySet _recycledEntities = new();
    private readonly EntitySet _takenEntities = new();

    public int TakenCount =>
        _takenEntities.Count;

    public Entity Take() {
        Entity entity;
        if (_recycledEntities.Count > 0) {
            Entity recycledEntity = Pop(_recycledEntities)!.Value;
            entity = new Entity(recycledEntity.Id, recycledEntity.Version + 1);
        } else if (_takenEntities.Count > 0) {
            int id = Peek(_takenEntities)!.Value.Id + 1;
            entity = new Entity(id);
        } else {
            entity = new Entity(0);
        }
        _takenEntities.Add(entity);
        return entity;
    }

    public bool Taken(Entity entity) =>
        _takenEntities.Contains(entity);

    public bool Recycle(Entity entity) {
        if (_takenEntities.Remove(entity)) {
            _recycledEntities.Add(entity);
            return true;
        } else {
            return false;
        }
    }

    private static Entity? Peek(EntitySet entitySet) =>
        entitySet.Count > 0
            ? entitySet.Keys[^1]
            : null;

    private static Entity? Pop(EntitySet entitySet) {
        if (entitySet.Count == 0) {
            return null;
        }
        Entity entity = entitySet.Keys[^1];
        entitySet.Remove(entity);
        return entity;
    }
}
