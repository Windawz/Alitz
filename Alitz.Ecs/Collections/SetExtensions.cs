﻿using System;

namespace Alitz.Collections;
public static class SetExtensions
{
    public static void Add<T>(this ISet<T> set, T value)
    {
        if (!set.TryAdd(value))
        {
            throw new ArgumentOutOfRangeException(nameof(value));
        }
    }
}
