using System;
using System.Collections.Generic;

namespace Alitz.Collections;
public interface IDictionary<TKey>
{
    Type ValueType { get; }
    int Count { get; }
    IEnumerable<TKey> Keys { get; }
    IEnumerable<object> Values { get; }

    object this[TKey key] { get; set; }

    bool TryAdd(TKey key, object value);
    bool Contains(TKey key);
    bool Remove(TKey key);
    void Clear();
    bool TryGet(TKey key, out object value);
    bool TrySet(TKey key, object value);
}
