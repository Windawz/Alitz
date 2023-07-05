using System.Collections.Generic;

namespace Alitz.Collections;
public interface IColumn<TComponent> : IColumn where TComponent : struct
{
    int Count { get; }
    IEnumerable<Entity> Entities { get; }
    IEnumerable<TComponent> Components { get; }

    TComponent this[Entity entity] { get; set; }

    bool TryAdd(Entity entity, TComponent component);
    bool TryGet(Entity entity, out TComponent component);
    bool TrySet(Entity entity, TComponent component);
    ref TComponent GetByRef(Entity entity);
}
