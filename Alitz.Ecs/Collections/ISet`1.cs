using System.Collections.Generic;

namespace Alitz.Ecs.Collections;
public interface ISet<T>
{
    int Count { get; }
    IEnumerable<T> Values { get; }

    bool TryAdd(T value);
    bool Contains(T value);
    bool Remove(T value);
    void Clear();
}
