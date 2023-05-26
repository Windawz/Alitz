using System;
using System.Collections.Generic;
using System.Linq;

namespace Alitz.Ecs;
public class SparseSet<TKey, TIndexProvider> 
    where TKey : struct 
    where TIndexProvider : struct, IIndexProvider<TKey> {
    
    private const int SparseFillValue = -1;

    private static readonly TIndexProvider IndexProvider = default;
    private readonly List<TKey> _dense = new();
    private readonly List<int> _sparse = new();

    public int Count =>
        _dense.Count;
    
    public IReadOnlyList<TKey> Keys =>
        _dense;

    public int Add(TKey key) {
        int sparseIndex = AsSparseIndex(key);
        ResizeSparseList(sparseIndex + 1);
        int denseIndex = TryGetDenseIndex(sparseIndex)
            ?? _sparse[sparseIndex];
        if (denseIndex == SparseFillValue) {
            denseIndex = _sparse[sparseIndex] = _dense.Count;
            _dense.Add(key);
            return denseIndex;
        } else {
            return -1;
        }
    }
    
    public bool Contains(TKey key) {
        int sparseIndex = AsSparseIndex(key);
        return sparseIndex >= 0
            && sparseIndex < _sparse.Count 
            && _sparse[sparseIndex] != SparseFillValue;
    }

    public bool Remove(TKey key) {
        int sparseIndex = AsSparseIndex(key);
        int? denseIndex = TryGetDenseIndex(sparseIndex);
        if (denseIndex is null) {
            return false;
        }
        
        SparseSetAlgorithms.SwapRemoveSparse(_sparse, sparseIndex, AsSparseIndex(_dense[^1]), SparseFillValue);
        SparseSetAlgorithms.SwapRemoveDense(_dense, denseIndex.Value);
        
        return true;
    }
    
    protected static int AsSparseIndex(TKey key) =>
        IndexProvider.AsIndex(key);
    
    protected int? TryGetDenseIndex(int sparseIndex) =>
        _sparse[sparseIndex] != SparseFillValue ? _sparse[sparseIndex] : null;
    
    private void ResizeSparseList(int count) {
        if (count < 0) {
            throw new ArgumentOutOfRangeException(nameof(count));
        }
        int currentCount = _sparse.Count;
        
        if (count < currentCount) {
            _sparse.RemoveRange(count, currentCount - count);
        } else if (count > currentCount) {
            _sparse.EnsureCapacity(count);
            _sparse.AddRange(Enumerable.Repeat(SparseFillValue, count - currentCount));
        }
    }
}
