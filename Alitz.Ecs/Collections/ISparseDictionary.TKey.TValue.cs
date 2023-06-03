﻿using System.Collections.Generic;

namespace Alitz.Ecs.Collections;
public interface ISparseDictionary<TKey, TValue>
{
    int Count { get; }
    IEnumerable<TKey> Keys { get; }
    IEnumerable<TValue> Values { get; }

    TValue this[TKey key] { get; set; }

    bool TryAdd(TKey key, TValue value);
    bool Contains(TKey key);
    bool Remove(TKey key);
    void Clear();
    bool TryGet(TKey key, out TValue value);
    bool TrySet(TKey key, TValue value);
    ref TValue GetByRef(TKey key);
}