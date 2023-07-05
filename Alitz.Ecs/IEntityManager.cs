using System.Collections.Generic;

namespace Alitz;
public interface IEntityManager
{
    IEnumerable<Entity> Entities { get; }

    Entity Create();
    bool Exists(Entity entity);
    void Destroy(Entity entity);
}
