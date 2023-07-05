using System.Collections.Generic;
using System.Linq;

namespace Alitz.Collections;
public abstract class Pool<T> : IPool<T>
{
    private readonly HashSet<T> _occupied = new();
    private readonly Stack<T> _scheduledForReuse = new();
    private T? _lastFetched;

    public IReadOnlyCollection<T> Occupied =>
        _occupied;

    public T Fetch()
    {
        T value;
        if (_scheduledForReuse.Count > 0)
        {
            var toBeReused = _scheduledForReuse.Pop();
            value = Reuse(toBeReused);
        }
        else
        {
            if (_lastFetched is null && _occupied.Count != 0)
            {
                _lastFetched = _occupied.Last();
            }

            if (_lastFetched is not null)
            {
                value = Next((T)_lastFetched);
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

    public bool Store(T value)
    {
        if (_occupied.Remove(value))
        {
            _scheduledForReuse.Push(value);
            return true;
        }
        return false;
    }

    public bool IsOccupied(T value) =>
        _occupied.Contains(value);

    protected abstract T Reuse(T toBeReused);
    protected abstract T Next(T last);
    protected abstract T New();
}
