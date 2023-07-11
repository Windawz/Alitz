using System.Collections.Generic;

namespace Alitz.Collections;
public partial class SparseColumn<TComponent>
{
    IEnumerator<KeyValuePair<Id, TComponent>> IEnumerable<KeyValuePair<Id, TComponent>>.GetEnumerator()
    {
        for (int i = 0; i < Count; i++)
        {
            yield return new KeyValuePair<Id, TComponent>(_denseEntities[i], _denseComponents[i]);
        }
    }
}
