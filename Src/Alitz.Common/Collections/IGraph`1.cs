using System.Collections.Generic;

namespace Alitz.Common.Collections;
public interface IGraph<T>
{
    T Value { get; }
    IReadOnlyCollection<IGraph<T>> Children { get; }
}