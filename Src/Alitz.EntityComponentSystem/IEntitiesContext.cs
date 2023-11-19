using System;
using System.Collections.Generic;

using Alitz.Common;
using Alitz.Common.Collections;

namespace Alitz.EntityComponentSystem;
public interface IEntitiesContext
{
    void RawForEach<TEntityEnumerator>(
        Func<IdPool, ITable, TEntityEnumerator> enumeratorFactory,
        Action<IEntityContext, ITable> action
    )
        where TEntityEnumerator : IEnumerator<Id>;
}