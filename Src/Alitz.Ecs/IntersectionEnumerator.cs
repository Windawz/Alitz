using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Alitz.Collections;

namespace Alitz;
internal readonly struct IntersectionEnumerator : IEnumerator<Id>
{
    public IntersectionEnumerator(IColumn column, params IColumn[] columns)
    {
        var shortestColumn = Enumerable.Repeat(column, 1).Concat(columns).MinBy(dict => dict.Count)!;

        _shortestEnumerator = shortestColumn.Entities.GetEnumerator();

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

    private readonly IEnumerator<Id> _shortestEnumerator;
    private readonly Func<Id, bool> _intersectionPredicate;

    object IEnumerator.Current =>
        Current;

    public Id Current =>
        _shortestEnumerator.Current;

    public bool MoveNext()
    {
        bool didMove;
        do
        {
            didMove = _shortestEnumerator.MoveNext();
        }
        while (didMove && !_intersectionPredicate(_shortestEnumerator.Current));
        return didMove;
    }

    void IEnumerator.Reset() =>
        _shortestEnumerator.Reset();

    void IDisposable.Dispose() =>
        _shortestEnumerator.Dispose();
}
