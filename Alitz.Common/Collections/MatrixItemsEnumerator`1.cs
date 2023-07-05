using System;

namespace Alitz.Collections;
public struct MatrixItemsEnumerator<T>
{
    private const int InvalidItemIndex = -1;

    internal MatrixItemsEnumerator(IMatrix<T> matrix)
    {
        _matrix = matrix;
    }

    private readonly IMatrix<T> _matrix;
    private int _itemIndex = InvalidItemIndex;

    public ref T Current
    {
        get
        {
            if (_itemIndex == InvalidItemIndex)
            {
                string typeName = typeof(MatrixItemsEnumerator<>).Name;
                throw new InvalidOperationException($"Cannot get current item of an exhausted {typeName}");
            }

            return ref _matrix[_itemIndex];
        }
    }

    public bool MoveNext()
    {
        if (_itemIndex == InvalidItemIndex || _itemIndex >= _matrix.Count)
        {
            _itemIndex = InvalidItemIndex;
            return false;
        }
        _itemIndex++;
        return true;
    }
}
