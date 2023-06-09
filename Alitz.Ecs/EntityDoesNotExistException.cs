using System;

namespace Alitz;
public class EntityDoesNotExistException : Exception
{
    public EntityDoesNotExistException(Entity entity)
    {
        Entity = entity;
    }

    public Entity Entity { get; }

    public override string Message =>
        $"Entity {Entity} does not exist";
}
