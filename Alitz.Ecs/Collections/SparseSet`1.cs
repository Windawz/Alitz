using System;
using System.Collections.Generic;

namespace Alitz.Collections;
using static Validation;

public class SparseSet<T> : ISet<T>, ISet
{
    public SparseSet(IndexExtractor<T> indexExtractor)
    {
        IndexExtractor = indexExtractor;
    }

    protected IList<int> Sparse { get; } = new List<int>();
    protected IList<T> Dense { get; } = new List<T>();
    protected IndexExtractor<T> IndexExtractor { get; }

    Type ISet.ValueType =>
        typeof(T);

    IEnumerable<object> ISet.Values
    {
        get
        {
            foreach (var value in Values)
            {
                yield return value!;
            }
        }
    }

    bool ISet.TryAdd(object value) =>
        TryAdd(ValidateType<T>(value));

    bool ISet.Remove(object value) =>
        Remove(ValidateType<T>(value));

    bool ISet.Contains(object value) =>
        Contains(ValidateType<T>(value));

    public int Count =>
        Dense.Count;

    public IEnumerable<T> Values =>
        Dense;

    public bool TryAdd(T value)
    {
        if (SparseSetAlgorithms.TryAddSparse(Sparse, value, IndexExtractor, Dense.Count, out _))
        {
            SparseSetAlgorithms.AddDense(Dense, value);
            return true;
        }
        return false;
    }

    public bool Contains(T value) =>
        SparseSetAlgorithms.Contains(Sparse, value, IndexExtractor);

    public bool Remove(T value)
    {
        if (SparseSetAlgorithms.TryGetSparseIndexBoundsChecked(value, IndexExtractor, Sparse.Count, out int sparseIndex)
            && SparseSetAlgorithms.TryGetDenseIndexBoundsChecked(Sparse, sparseIndex, out int denseIndex))
        {
            SparseSetAlgorithms.RemoveSparse(
                Sparse,
                sparseIndex,
                SparseSetAlgorithms.GetLastSparseIndex(Dense, IndexExtractor));
            SparseSetAlgorithms.RemoveDense(Dense, denseIndex);
            return true;
        }
        return false;
    }

    public void Clear()
    {
        SparseSetAlgorithms.ClearSparse(Sparse);
        SparseSetAlgorithms.ClearDense(Dense);
    }
}
