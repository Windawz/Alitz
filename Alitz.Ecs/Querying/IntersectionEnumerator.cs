using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Alitz;
internal readonly struct IntersectionEnumerator : IEnumerator<Entity>
{
    public IntersectionEnumerator(IDictionary<Entity> dictionary, params IDictionary<Entity>[] dictionaries)
    {
        var shortestDictionary = Enumerable.Repeat(dictionary, 1).Concat(dictionaries).MinBy(dict => dict.Count)!;

        _shortestEnumerator = shortestDictionary.Keys.GetEnumerator();

        _intersectionPredicate = Enumerable.Repeat(dictionary, 1)
            .Concat(dictionaries)
            .Where(dict => !ReferenceEquals(dict, shortestDictionary))
            .Select(dict => (Predicate<Entity>)dict.Contains)
            .Aggregate((left, right) => entity => left(entity) && right(entity));
    }

    private readonly IEnumerator<Entity> _shortestEnumerator;
    private readonly Predicate<Entity> _intersectionPredicate;

    object IEnumerator.Current =>
        Current;

    public Entity Current =>
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
