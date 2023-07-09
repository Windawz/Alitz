using System.Collections.Generic;

namespace Alitz;
public interface IEntityManager
{
    IReadOnlyCollection<Entity> Entities { get; }

    Entity Create();
    bool Exists(Entity entity);
    void Destroy(Entity entity);
}
