using System.Collections.Generic;

namespace Alitz.Collections;
public interface IPool<T>
{
    IReadOnlyCollection<T> Occupied { get; }

    T Fetch();
    bool Store(T value);
    bool IsOccupied(T value);
}
