using System;
using System.Collections.Generic;

namespace Alitz.Collections;
public interface IColumn
{
    Type ComponentType { get; }
    int Count { get; }
    IEnumerable<Entity> Entities { get; }

    bool Contains(Entity entity);
    bool Remove(Entity entity);
    void Clear();
}
