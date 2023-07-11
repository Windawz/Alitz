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
        var shortestDictionary = Enumerable.Repeat(column, 1).Concat(columns).MinBy(dict => dict.Count)!;

        _shortestEnumerator = shortestDictionary.Entities.GetEnumerator();

        _intersectionPredicate = Enumerable.Repeat(column, 1)
            .Concat(columns)
            .Where(dict => !ReferenceEquals(dict, shortestDictionary))
            .Select(dict => (Predicate<Id>)dict.Contains)
            .Aggregate((left, right) => entity => left(entity) && right(entity));
    }

    private readonly IEnumerator<Id> _shortestEnumerator;
    private readonly Predicate<Id> _intersectionPredicate;

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
