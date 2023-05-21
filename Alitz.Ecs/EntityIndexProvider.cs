namespace Alitz.Ecs; 
public struct EntityIndexProvider : IIndexProvider<Entity> {
    public int AsIndex(Entity value) =>
        value.Id;
}
