using System.Collections.Generic;
using System.Linq;

namespace Alitz.Collections;
public class IdPool
{
    private readonly HashSet<Id> _occupied = new();
    private readonly Stack<Id> _scheduledForReuse = new();
    private bool _isLastFetchedPresent;
    private Id _lastFetched;

    public IReadOnlyCollection<Id> Occupied =>
        _occupied;

    public Id Fetch()
    {
        Id value;
        if (_scheduledForReuse.Count > 0)
        {
            var toBeReused = _scheduledForReuse.Pop();
            value = Reuse(toBeReused);
        }
        else
        {
            if (!_isLastFetchedPresent && _occupied.Count != 0)
            {
                _lastFetched = _occupied.Last();
                _isLastFetchedPresent = true;
            }

            if (_isLastFetchedPresent)
            {
                value = Next(_lastFetched);
            }
            else
            {
                value = New();
            }
        }

        _occupied.Add(value);
        _lastFetched = value;
        return value;
    }

    public bool Store(Id value)
    {
        if (_occupied.Remove(value))
        {
            _scheduledForReuse.Push(value);
            return true;
        }
        return false;
    }

    public bool IsOccupied(Id value) =>
        _occupied.Contains(value);

    private Id Reuse(Id toBeReused) =>
        new(toBeReused.Index, toBeReused.Version + 1);

    private Id Next(Id last) =>
        new(last.Index + 1, Id.MinVersion);

    private Id New() =>
        new(Id.MinIndex, Id.MinVersion);
}
