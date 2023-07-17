using System.Collections.Generic;

namespace Alitz.Collections;
public interface IColumn<TComponent> : IColumn where TComponent : struct
{
    IEnumerable<TComponent> Components { get; }

    TComponent this[Id entity] { get; set; }

    bool TryAdd(Id entity, TComponent component);
    bool TryGet(Id entity, out TComponent component);
    bool TrySet(Id entity, TComponent component);
    ref TComponent GetByRef(Id entity);
}
