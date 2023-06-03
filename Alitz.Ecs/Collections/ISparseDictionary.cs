using System;
using System.Collections.Generic;

namespace Alitz.Ecs.Collections;
public interface ISparseDictionary {
    Type KeyType { get; }
    Type ValueType { get; }
    int Count { get; }
    IEnumerable<object> Keys { get; }
    IEnumerable<object> Values { get; }

    object this[object key] { get; set; }

    bool TryAdd(object key, object value);
    bool Contains(object key);
    bool Remove(object key);
    void Clear();
    bool TryGet(object key, out object value);
    bool TrySet(object key, object value);
}
