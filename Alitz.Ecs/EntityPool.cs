namespace Alitz.Ecs; 
public class EntityPool {
    private readonly StackEntitySet _takenEntities = new();
    private readonly StackEntitySet _recycledEntities = new();
    
    public int TakenCount =>
        _takenEntities.Count;
    
    public Entity Take() {
        Entity entity;
        if (_recycledEntities.Count > 0) {
            Entity recycledEntity = _recycledEntities.Pop()!.Value;
            entity = new Entity(recycledEntity.Id, recycledEntity.Version + 1);
        } else if (_takenEntities.Count > 0) {
            int id = _takenEntities.Peek()!.Value.Id + 1;
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
    
    private class StackEntitySet : EntitySet {
        public Entity? Peek() =>
            Count > 0
            ? DenseList[^1]
            : null;
        
        public Entity? Pop() {
            if (Count == 0) {
                return null;
            }
            Entity entity = DenseList[^1];
            TryRemoveEntity(entity);
            return entity;
        }
    }
}
