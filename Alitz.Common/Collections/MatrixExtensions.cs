using System;
using System.Collections.Generic;

namespace Alitz.Collections;
public static class MatrixExtensions
{
    public static MatrixColumnView<T> Column<T>(this IMatrix<T> matrix, int index) =>
        new(matrix, index);

    public static MatrixRowView<T> Row<T>(this IMatrix<T> matrix, int index) =>
        new(matrix, index);

    public static IEnumerable<T> EnumerateRow<T>(this IMatrix<T> matrix, int index)
    {
        var enumerator = new MatrixRowView<T>(matrix, index).GetEnumerator();
        while (enumerator.MoveNext())
        {
            yield return enumerator.Current;
        }
    }

    public static IEnumerable<T> EnumerateColumn<T>(this IMatrix<T> matrix, int index)
    {
        var enumerator = new MatrixColumnView<T>(matrix, index).GetEnumerator();
        while (enumerator.MoveNext())
        {
            yield return enumerator.Current;
        }
    }

    public static IEnumerable<T> EnumerateItems<T>(this IMatrix<T> matrix)
    {
        for (int i = 0; i < matrix.Height; i++)
        {
            for (int j = 0; j < matrix.Width; j++)
            {
                yield return matrix[j, i];
            }
        }
    }

    public static void FillArea<T>(this IMatrix<T> matrix, int x, int y, int width, int height, T value)
    {
        Fill_ThrowIfOutOfRange(matrix, x, y, width, height);
        for (int i = y; i < y + height; i++)
        {
            for (int j = x; j < x + width; j++)
            {
                matrix[j, i] = value;
            }
        }
    }

    public static void FillAreaFrom<T>(this IMatrix<T> matrix, int x, int y, int width, int height, Func<T> factory)
    {
        Fill_ThrowIfOutOfRange(matrix, x, y, width, height);
        for (int i = y; i < y + height; i++)
        {
            for (int j = x; j < x + width; j++)
            {
                matrix[j, i] = factory();
            }
        }
    }

    public static MatrixItemsEnumerator<T> GetEnumerator<T>(this IMatrix<T> matrix) =>
        new(matrix);

    private static void Fill_ThrowIfOutOfRange<T>(IMatrix<T> matrix, int x, int y, int width, int height)
    {
        if (x < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(x));
        }
        if (y < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(y));
        }
        if (width < 0 || x + width > matrix.Width)
        {
            throw new ArgumentOutOfRangeException(nameof(width));
        }
        if (height < 0 || y + height > matrix.Height)
        {
            throw new ArgumentOutOfRangeException(nameof(height));
        }
    }
}
