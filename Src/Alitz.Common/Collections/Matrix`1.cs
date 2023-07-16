using System;

namespace Alitz.Collections;
public class Matrix<T> : IMatrix<T>
{
    public Matrix(int width, int height)
    {
        if (width < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(width));
        }
        if (height < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(height));
        }
        Resize(width, height);
    }

    private T[] _elems = null!;

    public int Count =>
        _elems.Length;

    public int Width { get; private set; }

    public int Height { get; private set; }

    public ref T this[int x, int y] =>
        ref _elems[Matrix.Index(x, y, Width)];

    public ref T this[int index] =>
        ref _elems[index];

    public void Fill(T value) =>
        Array.Fill(_elems, value);

    public void Resize(int width, int height)
    {
        int length = width * height;
        _elems = new T[length];
        Width = width;
        Height = height;
    }
}
