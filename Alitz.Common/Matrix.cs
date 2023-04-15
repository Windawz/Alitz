using System;
using System.Collections.Generic;

namespace Alitz;
public class Matrix<T> {
    public Matrix(int width, int height) {
        Resize(width, height);
    }

    public Matrix(int width, int height, T value) : this(width, height) {
        Array.Fill(_elems, value);
    }

    private T[] _elems = null!;

    public int Count => _elems.Length;
    public int Width { get; private set; }
    public int Height { get; private set; }

    public IEnumerable<IEnumerable<T>> Rows {
        get {
            for (int i = 0; i < Height; i++) {
                yield return Row(i);
            }
        }
    }

    public IEnumerable<IEnumerable<T>> Columns {
        get {
            for (int i = 0; i < Width; i++) {
                yield return Column(i);
            }
        }
    }
    
    public IEnumerable<T> Elements {
        get {
            for (int i = 0; i < _elems.Length; i++) {
                yield return _elems[i];
            }
        }
    }

    public T this[int x, int y] {
        get => _elems[GetAbsoluteIndex(x, y)];
        set => _elems[GetAbsoluteIndex(x, y)] = value;
    }
    
    public T this[int index] {
        get => _elems[index];
        set => _elems[index] = value;
    }

    public void Resize(int width, int height) {
        int length = width * height;
        _elems = new T[length];
        Width = width;
        Height = height;
    }

    public int GetAbsoluteIndex(int x, int y) =>
        y * Width + x;

    public IEnumerable<T> Row(int index) {
        for (int i = 0; i < Width; i++) {
            yield return _elems[GetAbsoluteIndex(i, index)];
        }
    }

    public IEnumerable<T> Column(int index) {
        for (int i = 0; i < Height; i++) {
            yield return _elems[GetAbsoluteIndex(index, i)];
        }
    }
}
