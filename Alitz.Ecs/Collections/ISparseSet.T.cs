using System;
using System.Collections.Generic;

namespace Alitz.Ecs.Collections;
public interface ISparseSet<T> : ISparseSet
{
    new IEnumerable<T> Values { get; }
    Type ISparseSet.ValueType =>
        typeof(T);

    IEnumerable<object> ISparseSet.Values
    {
        get
        {
            foreach (var value in Values)
            {
                yield return value!;
            }
        }
    }

    bool ISparseSet.TryAdd(object value) =>
        TryAdd((T)value);

    bool ISparseSet.Contains(object value) =>
        Contains((T)value);

    bool ISparseSet.Remove(object value) =>
        Remove((T)value);

    bool TryAdd(T value);
    bool Contains(T value);
    bool Remove(T value);
}
