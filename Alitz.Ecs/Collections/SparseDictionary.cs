using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Alitz.Ecs.Collections;
using static Validation;

public class SparseDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IDictionary
{
    public SparseDictionary(IndexExtractor<TKey> keyIndexExtractor)
    {
        _keyIndexExtractor = keyIndexExtractor;
    }

    private readonly IList<TKey> _denseKeys = new List<TKey>();
    // Type is List, not IList, to allow getting values by reference.
    // CollectionsMarshal.AsSpan only accepts Lists, not ILists.
    private readonly List<TValue> _denseValues = new();
    private readonly IndexExtractor<TKey> _keyIndexExtractor;

    private readonly IList<int> _sparse = new List<int>();

    Type IDictionary.KeyType =>
        typeof(TKey);

    Type IDictionary.ValueType =>
        typeof(TValue);

    IEnumerable<object> IDictionary.Keys
    {
        get
        {
            foreach (var key in Keys)
            {
                yield return key!;
            }
        }
    }

    IEnumerable<object> IDictionary.Values
    {
        get
        {
            foreach (var value in Values)
            {
                yield return value!;
            }
        }
    }

    object IDictionary.this[object key]
    {
        get => this[ValidateType<TKey>(key)]!;
        set => this[ValidateType<TKey>(key)] = ValidateType<TValue>(value);
    }

    bool IDictionary.TryAdd(object key, object value) =>
        TryAdd(ValidateType<TKey>(key, nameof(key)), ValidateType<TValue>(value, nameof(value)));

    bool IDictionary.Contains(object key) =>
        Contains(ValidateType<TKey>(key));

    bool IDictionary.Remove(object key) =>
        Remove(ValidateType<TKey>(key));

    bool IDictionary.TryGet(object key, out object value)
    {
        if (TryGet(ValidateType<TKey>(key), out var typedValue))
        {
            value = typedValue!;
            return true;
        }
        value = default!;
        return false;
    }

    bool IDictionary.TrySet(object key, object value) =>
        TrySet(ValidateType<TKey>(key), ValidateType<TValue>(value));

    public TValue this[TKey key]
    {
        get
        {
            if (TryGet(key, out var value))
            {
                return value;
            }
            throw new ArgumentOutOfRangeException(nameof(key));
        }
        set
        {
            if (!TryAdd(key, value))
            {
                bool hasSet = TrySet(key, value);
                Debug.Assert(hasSet);
            }
        }
    }

    public int Count =>
        _denseKeys.Count;

    public IEnumerable<TKey> Keys =>
        _denseKeys;

    public IEnumerable<TValue> Values =>
        _denseValues;

    public bool TryAdd(TKey key, TValue value)
    {
        bool hasAdded = SparseSetAlgorithms.TryAddSparse(_sparse, key, _keyIndexExtractor, _denseKeys.Count, out _);
        if (hasAdded)
        {
            SparseSetAlgorithms.AddDense(_denseKeys, key);
            SparseSetAlgorithms.AddDense(_denseValues, value);
        }
        return hasAdded;
    }

    public bool Contains(TKey key) =>
        SparseSetAlgorithms.Contains(_sparse, key, _keyIndexExtractor);

    public bool Remove(TKey key)
    {
        if (SparseSetAlgorithms.TryGetSparseIndexBoundsChecked(key, _keyIndexExtractor, _sparse.Count, out int sparseIndex)
            && SparseSetAlgorithms.TryGetDenseIndexBoundsChecked(_sparse, sparseIndex, out int denseIndex))
        {
            SparseSetAlgorithms.RemoveSparse(
                _sparse,
                sparseIndex,
                SparseSetAlgorithms.GetLastSparseIndex(_denseKeys, _keyIndexExtractor));
            SparseSetAlgorithms.RemoveDense(_denseKeys, denseIndex);
            SparseSetAlgorithms.RemoveDense(_denseValues, denseIndex);
            return true;
        }
        return false;
    }

    public void Clear()
    {
        SparseSetAlgorithms.ClearSparse(_sparse);
        SparseSetAlgorithms.ClearDense(_denseKeys);
        SparseSetAlgorithms.ClearDense(_denseValues);
    }

    public bool TryGet(TKey key, out TValue value)
    {
        if (SparseSetAlgorithms.TryGetDenseIndexBoundsChecked(_sparse, key, _keyIndexExtractor, out int denseIndex))
        {
            value = _denseValues[denseIndex];
            return true;
        }
        value = default!;
        return false;
    }

    public bool TrySet(TKey key, TValue value)
    {
        if (SparseSetAlgorithms.TryGetDenseIndexBoundsChecked(_sparse, key, _keyIndexExtractor, out int denseIndex))
        {
            _denseValues[denseIndex] = value;
            return true;
        }
        return false;
    }

    public ref TValue GetByRef(TKey key)
    {
        if (SparseSetAlgorithms.TryGetDenseIndexBoundsChecked(_sparse, key, _keyIndexExtractor, out int denseIndex))
        {
            return ref CollectionsMarshal.AsSpan(_denseValues)[denseIndex];
        }
        throw new ArgumentOutOfRangeException(nameof(key));
    }
}
