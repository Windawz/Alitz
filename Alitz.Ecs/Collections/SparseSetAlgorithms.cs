using System;
using System.Collections.Generic;

namespace Alitz.Ecs.Collections;
internal static class SparseSetAlgorithms
{
    private const int SparseFillValue = -1;

    public static bool TryAddSparse<T>(
        IList<int> sparse,
        T value,
        IndexExtractor<T> indexExtractor,
        int denseCount,
        out int denseIndex
    )
    {
        int sparseIndex = GetSparseIndex(value, indexExtractor);
        ResizeSparse(sparse, sparseIndex + 1);
        bool hasAdded;
        if (!TryGetDenseIndexBoundsChecked(sparse, sparseIndex, out denseIndex))
        {
            denseIndex = sparse[sparseIndex] = denseCount;
            hasAdded = true;
        }
        else
        {
            hasAdded = false;
        }
        return hasAdded;
    }

    public static void AddDense<T>(IList<T> dense, T value) =>
        dense.Add(value);

    public static bool Contains<T>(IList<int> sparse, T value, IndexExtractor<T> indexExtractor) =>
        TryGetDenseIndexBoundsChecked(sparse, GetSparseIndex(value, indexExtractor), out _);

    public static void RemoveSparse(IList<int> sparse, int sparseIndex, int lastSparseIndex)
    {
        sparse[lastSparseIndex] = sparse[sparseIndex];
        sparse[sparseIndex] = SparseFillValue;
    }

    public static void RemoveDense<T>(IList<T> dense, int denseIndex)
    {
        dense[denseIndex] = dense[^1];
        dense.RemoveAt(dense.Count - 1);
    }

    public static void ClearSparse(IList<int> sparse) =>
        sparse.Clear();

    public static void ClearDense<T>(IList<T> dense) =>
        dense.Clear();

    public static void ResizeSparse(IList<int> sparse, int desiredCount)
    {
        if (desiredCount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(desiredCount));
        }
        while (sparse.Count > desiredCount)
        {
            sparse.RemoveAt(desiredCount);
        }
        if (sparse is List<int> sparseList)
        {
            sparseList.EnsureCapacity(desiredCount);
        }
        while (sparse.Count < desiredCount)
        {
            sparse.Add(SparseFillValue);
        }
    }

    public static int GetSparseIndex<T>(T value, IndexExtractor<T> indexExtractor) =>
        indexExtractor.Extract(value);

    public static bool TryGetSparseIndexBoundsChecked<T>(
        T value,
        IndexExtractor<T> indexExtractor,
        int sparseCount,
        out int sparseIndex
    )
    {
        sparseIndex = GetSparseIndex(value, indexExtractor);
        return IsSparseIndexInBounds(sparseIndex, sparseCount);
    }

    public static bool TryGetDenseIndexBoundsChecked<T>(
        IList<int> sparse,
        T value,
        IndexExtractor<T> indexExtractor,
        out int denseIndex
    )
    {
        denseIndex = default;
        int sparseIndex = GetSparseIndex(value, indexExtractor);
        if (TryGetDenseIndexBoundsChecked(sparse, sparseIndex, out denseIndex))
        {
            return true;
        }
        return false;
    }

    public static bool TryGetDenseIndexBoundsChecked(IList<int> sparse, int sparseIndex, out int denseIndex)
    {
        denseIndex = default;
        if (!IsSparseIndexInBounds(sparseIndex, sparse.Count))
        {
            return false;
        }
        if (TryGetDenseIndex(sparse, sparseIndex, out denseIndex))
        {
            return true;
        }
        return false;
    }

    public static bool TryGetDenseIndex(IList<int> sparse, int sparseIndex, out int denseIndex)
    {
        denseIndex = GetDenseIndex(sparse, sparseIndex);
        if (denseIndex == SparseFillValue)
        {
            return false;
        }
        return true;
    }

    public static int GetDenseIndex(IList<int> sparse, int sparseIndex) =>
        sparse[sparseIndex];

    public static bool IsSparseIndexInBounds(int sparseIndex, int sparseCount) =>
        sparseIndex >= 0 && sparseIndex < sparseCount;

    public static int GetLastSparseIndex<T>(IList<T> dense, IndexExtractor<T> indexExtractor) =>
        GetSparseIndex(dense[^1], indexExtractor);
}
