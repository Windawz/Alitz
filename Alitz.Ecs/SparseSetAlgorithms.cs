using System;
using System.Collections.Generic;

namespace Alitz.Ecs; 
internal static class SparseSetAlgorithms {
    public static void SwapRemoveSparse(List<int> sparse, int sparseIndex, int lastSparseIndex, int sparseFillValue) {
        sparse[sparseIndex] = sparse[lastSparseIndex];
        sparse[lastSparseIndex] = sparseFillValue;
    }
    
    public static void SwapRemoveDense<T>(List<T> dense, int denseIndex) {
        if (denseIndex < 0 || denseIndex >= dense.Count) {
            throw new ArgumentOutOfRangeException();
        }
        dense[denseIndex] = dense[^1];
        dense.RemoveAt(dense.Count - 1);
    }
}
