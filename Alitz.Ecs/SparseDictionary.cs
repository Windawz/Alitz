using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Alitz.Ecs; 
public class SparseDictionary<TKey, TIndexProvider, TValue>
    where TKey : struct
    where TIndexProvider : struct, IIndexProvider<TKey>
    where TValue : struct {
    
    private readonly InnerSparseSet _sparseSet = new();
    private readonly List<TValue> _values = new();
    
    public int Count =>
        _values.Count;
    
    public IReadOnlyList<TKey> Keys =>
        _sparseSet.Keys;
    
    public IReadOnlyList<TValue> Values =>
        _values;
    
    public int Add(TKey key) =>
        Add(key, default);
    
    public int Add(TKey key, TValue value) {
        int index = _sparseSet.Add(key);
        _values.Add(value);
        return index;
    }
    
    public bool Contains(TKey key) {
        return _sparseSet.Contains(key);
    }
    
    public ref TValue Get(TKey key) {
        int index = _sparseSet.TryGetIndex(key)
            ?? throw new ArgumentOutOfRangeException(nameof(key));
        return ref CollectionsMarshal.AsSpan(_values)[index];
    }
    
    public bool Remove(TKey key) {
        int? denseIndex = _sparseSet.TryGetIndex(key);
        if (denseIndex is null) {
            return false;
        }
        SparseSetAlgorithms.SwapRemoveDense(_values, denseIndex.Value);
        _sparseSet.Remove(key);
        return true;
    }
    
    private class InnerSparseSet : SparseSet<TKey, TIndexProvider> {
        public int? TryGetIndex(TKey key) {
            return TryGetDenseIndex(AsSparseIndex(key));
        }
    }
}
