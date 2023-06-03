using System;
using System.Collections.Generic;

namespace Alitz.Ecs.Collections;
public interface ISparseDictionary<TKey, TValue> : ISparseDictionary {
    new IEnumerable<TKey> Keys { get; }
    new IEnumerable<TValue> Values { get; }

    TValue this[TKey key] { get; set; }

    Type ISparseDictionary.KeyType =>
        typeof(TKey);

    Type ISparseDictionary.ValueType =>
        typeof(TValue);

    IEnumerable<object> ISparseDictionary.Keys {
        get {
            foreach (var key in Keys) {
                yield return key!;
            }
        }
    }

    IEnumerable<object> ISparseDictionary.Values {
        get {
            foreach (var value in Values) {
                yield return value!;
            }
        }
    }

    object ISparseDictionary.this[object key] {
        get => this[(TKey)key]!;
        set => this[(TKey)key] = (TValue)value;
    }

    bool ISparseDictionary.TryAdd(object key, object value) =>
        TryAdd((TKey)key, (TValue)value);

    bool ISparseDictionary.Contains(object key) =>
        Contains((TKey)key);

    bool ISparseDictionary.Remove(object key) =>
        Remove((TKey)key);

    bool ISparseDictionary.TryGet(object key, out object value) =>
        TryGet((TKey)key, out value);

    bool ISparseDictionary.TrySet(object key, object value) =>
        TrySet((TKey)key, (TValue)value);

    bool TryAdd(TKey key, TValue value);
    bool Contains(TKey key);
    bool Remove(TKey key);
    bool TryGet(TKey key, out TValue value);
    bool TrySet(TKey key, TValue value);
    ref TValue GetByRef(TKey key);
}
