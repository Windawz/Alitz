using System;
using System.Collections.Generic;

using Alitz.Collections;

namespace Alitz;
public class EntityComponentSystemOptions
{
    public Func<IDictionary<Type, IColumn>> ColumnTableFactory { get; init; } = () => new Dictionary<Type, IColumn>();
    public IColumnFactory ColumnFactory { get; init; } = new DefaultColumnFactory();

    private class DefaultColumnFactory : IColumnFactory
    {
        public IColumn<TComponent> Create<TComponent>() where TComponent : struct =>
            new SparseColumn<TComponent>();
    }
}
