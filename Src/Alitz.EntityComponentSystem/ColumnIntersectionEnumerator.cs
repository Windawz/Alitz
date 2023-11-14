using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Alitz.Common;


namespace Alitz.EntityComponentSystem;
internal readonly struct ColumnIntersectionEnumerator : IEnumerator<Id>
{
    public ColumnIntersectionEnumerator(params IColumn[] columns)
    {
        _columns = columns;
        var shortestColumn = _columns.MinBy(c => c.Count)!;
        _entityEnumerator = shortestColumn.Entities.GetEnumerator();
    }

    private readonly IEnumerator<Id> _entityEnumerator;
    private readonly IColumn[] _columns;

    public Id Current =>
        _entityEnumerator.Current;

    object IEnumerator.Current =>
        Current;

    public bool MoveNext()
    {
        bool didMove;
        do
        {
            didMove = _entityEnumerator.MoveNext();
        }
        while (didMove && !IsContainedByAllColumns(_entityEnumerator.Current));
        return didMove;
    }

    private bool IsContainedByAllColumns(Id entity)
    {
        for (int i = 0; i < _columns.Length; i++)
        {
            if (!_columns[i].Contains(entity))
            {
                return false;
            }
        }
        return true;
    }

    void IEnumerator.Reset() =>
        _entityEnumerator.Reset();

    void IDisposable.Dispose() =>
        _entityEnumerator.Dispose();
}
