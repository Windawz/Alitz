using System.Collections.Generic;

namespace Alitz.Ecs.Collections;
public class SparseSet<T> : ISparseSet<T>
{
    public SparseSet(IndexExtractor<T> indexExtractor)
    {
        IndexExtractor = indexExtractor;
    }

    protected IList<int> Sparse { get; } = new List<int>();
    protected IList<T> Dense { get; } = new List<T>();
    protected IndexExtractor<T> IndexExtractor { get; }

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
