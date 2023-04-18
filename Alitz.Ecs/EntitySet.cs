namespace Alitz.Ecs;
public class EntitySet : SparseSet {
    public void Add(Entity entity) {
        TryAddEntity(entity);
    }
    
    public bool Remove(Entity entity) {
        return TryRemoveEntity(entity) != InvalidDenseListIndex;
    }
}
