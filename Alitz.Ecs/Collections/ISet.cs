using System;
using System.Collections.Generic;

namespace Alitz.Ecs.Collections;
public interface ISet
{
    Type ValueType { get; }
    int Count { get; }
    IEnumerable<object> Values { get; }

    bool TryAdd(object value);
    bool Remove(object value);
    bool Contains(object value);
    void Clear();
}
