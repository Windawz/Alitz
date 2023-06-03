using System;

namespace Alitz.Ecs.Collections;
public static class SparseSetExtensions
{
    public static void Add<T>(this ISparseSet<T> set, T value)
    {
        if (!set.TryAdd(value))
        {
            throw new ArgumentOutOfRangeException(nameof(value));
        }
    }
}
