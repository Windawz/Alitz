using System.Collections;
using System.Collections.Generic;

namespace Alitz.Collections;
public partial class SparseColumn<TComponent>
{
    IEnumerator IEnumerable.GetEnumerator() =>
        ((IEnumerable<KeyValuePair<Id, TComponent>>)this).GetEnumerator();
}
