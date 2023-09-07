using System;
using System.Collections.Generic;

using Alitz.Common;

namespace Alitz.Ecs.Collections;
public interface IColumn
{
    Type ComponentType { get; }
    int Count { get; }
    IEnumerable<Id> Entities { get; }

    bool Contains(Id entity);
    bool Remove(Id entity);
    void Clear();
}
