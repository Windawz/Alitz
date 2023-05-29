namespace Alitz.Ecs.Collections;
public struct EntityIndexProvider : IIndexProvider<Entity> {
    public int AsIndex(Entity value) =>
        value.Id;
}
