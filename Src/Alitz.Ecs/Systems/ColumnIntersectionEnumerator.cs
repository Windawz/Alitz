using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Alitz.Collections;

namespace Alitz.Systems;
internal readonly struct ColumnIntersectionEnumerator : IEnumerator<Id>
{
    public ColumnIntersectionEnumerator(IColumn column, params IColumn[] columns)
    {
        var shortestColumn = Enumerable.Repeat(column, 1).Concat(columns).MinBy(dict => dict.Count)!;

        _entityEnumerator = shortestColumn.Entities.GetEnumerator();

        _intersectionPredicate = Enumerable.Repeat(column, 1)
            .Concat(columns)
            .Where(c => !ReferenceEquals(c, shortestColumn))
            .Select(c => (Func<Id, bool>)c.Contains)
            .Aggregate(
                (leftColumnContains, rightColumnContains) =>
                {
                    return entity => leftColumnContains(entity) && rightColumnContains(entity);
                });
    }

    private readonly IEnumerator<Id> _entityEnumerator;
    private readonly Func<Id, bool> _intersectionPredicate;

    object IEnumerator.Current =>
        Current;

    public Id Current =>
        _entityEnumerator.Current;

    public bool MoveNext()
    {
        bool didMove;
        do
        {
            didMove = _entityEnumerator.MoveNext();
        }
        while (didMove && !_intersectionPredicate(_entityEnumerator.Current));
        return didMove;
    }

    void IEnumerator.Reset() =>
        _entityEnumerator.Reset();

    void IDisposable.Dispose() =>
        _entityEnumerator.Dispose();
}
