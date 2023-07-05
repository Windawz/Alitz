using System;

namespace Alitz.Collections;
public readonly struct MatrixRowView<T>
{
    internal MatrixRowView(IMatrix<T> matrix, int index)
    {
        if (index < 0 || index >= matrix.Height)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        _matrix = matrix;
        _rowIndex = index;
    }

    private readonly IMatrix<T> _matrix;
    private readonly int _rowIndex;

    public ref T this[int columnIndex]
    {
        get
        {
            if (columnIndex < 0 || columnIndex >= _matrix.Width)
            {
                throw new ArgumentOutOfRangeException(nameof(columnIndex));
            }

            return ref _matrix[columnIndex, _rowIndex];
        }
    }

    public Enumerator GetEnumerator() =>
        new(this);

    public struct Enumerator
    {
        private const int InvalidColumnIndex = -1;

        internal Enumerator(MatrixRowView<T> view)
        {
            _view = view;
        }

        private readonly MatrixRowView<T> _view;
        private int _columnIndex = InvalidColumnIndex;

        public ref T Current
        {
            get
            {
                if (_columnIndex == InvalidColumnIndex)
                {
                    string typeName = typeof(Enumerator).DeclaringType?.Name ?? "" + typeof(Enumerator).Name;
                    throw new InvalidOperationException($"Cannot get current item of an exhausted {typeName}");
                }

                return ref _view[_columnIndex];
            }
        }

        public bool MoveNext()
        {
            if (_columnIndex == InvalidColumnIndex || _columnIndex >= _view._matrix.Width)
            {
                _columnIndex = InvalidColumnIndex;
                return false;
            }
            _columnIndex++;
            return true;
        }
    }
}
