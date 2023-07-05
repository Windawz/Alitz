namespace Alitz.Collections;
public class EntityPool : Pool<Entity>
{
    protected override Entity Reuse(Entity toBeReused) =>
        new(toBeReused.Id, toBeReused.Version + 1);

    protected override Entity Next(Entity last) =>
        new(last.Id + 1);

    protected override Entity New() =>
        new(0);
}
