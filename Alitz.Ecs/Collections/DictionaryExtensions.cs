using System;

namespace Alitz.Collections;
public static class DictionaryExtensions
{
    public static void Add<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) =>
        dictionary.Add(key, default!);

    public static void Add<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
    {
        if (!dictionary.TryAdd(key, value))
        {
            throw new ArgumentOutOfRangeException(nameof(key));
        }
    }

    public static bool TryAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) =>
        dictionary.TryAdd(key, default!);

    public static TValue Get<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
    {
        if (dictionary.TryGet(key, out var value))
        {
            return value;
        }
        throw new ArgumentOutOfRangeException(nameof(key));
    }

    public static void Set<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
    {
        if (!dictionary.TrySet(key, value))
        {
            throw new ArgumentOutOfRangeException(nameof(key));
        }
    }
}
