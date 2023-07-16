using System;

namespace Alitz.Collections;
public partial class SparseColumn<TComponent>
{
    Type IColumn.ComponentType =>
        typeof(TComponent);
}
