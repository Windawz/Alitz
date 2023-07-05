using System;

namespace Alitz.Collections;
public readonly struct MatrixColumnView<T>
{
    internal MatrixColumnView(IMatrix<T> matrix, int index)
    {
        if (index < 0 || index >= matrix.Width)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }
        _matrix = matrix;
        _columnIndex = index;
    }

    private readonly IMatrix<T> _matrix;
    private readonly int _columnIndex;

    public ref T this[int rowIndex]
    {
        get
        {
            if (rowIndex < 0 || rowIndex >= _matrix.Height)
            {
                throw new ArgumentOutOfRangeException(nameof(rowIndex));
            }
            return ref _matrix[_columnIndex, rowIndex];
        }
    }

    public Enumerator GetEnumerator() =>
        new(this);

    public struct Enumerator
    {
        private const int InvalidRowIndex = -1;

        internal Enumerator(MatrixColumnView<T> view)
        {
            _view = view;
        }

        private readonly MatrixColumnView<T> _view;
        private int _rowIndex = InvalidRowIndex;

        public ref T Current
        {
            get
            {
                if (_rowIndex == InvalidRowIndex)
                {
                    string typeName = typeof(Enumerator).DeclaringType?.Name ?? "" + typeof(Enumerator).Name;
                    throw new InvalidOperationException($"Cannot get current item of an exhausted {typeName}");
                }

                return ref _view[_rowIndex];
            }
        }

        public bool MoveNext()
        {
            if (_rowIndex == InvalidRowIndex || _rowIndex >= _view._matrix.Height)
            {
                _rowIndex = InvalidRowIndex;
                return false;
            }
            _rowIndex++;
            return true;
        }
    }
}
