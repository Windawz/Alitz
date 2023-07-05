using System.Collections.Generic;

namespace Alitz.Collections;
public partial class SparseColumn<TComponent>
{
    IEnumerator<KeyValuePair<Entity, TComponent>> IEnumerable<KeyValuePair<Entity, TComponent>>.GetEnumerator()
    {
        for (int i = 0; i < Count; i++)
        {
            yield return new KeyValuePair<Entity, TComponent>(_denseEntities[i], _denseComponents[i]);
        }
    }
}
