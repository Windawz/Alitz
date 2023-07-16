using System.Collections.Generic;

namespace Alitz.Collections;
public partial class SparseColumn<TComponent>
{
    IEnumerable<Id> IReadOnlyDictionary<Id, TComponent>.Keys =>
        Entities;

    IEnumerable<TComponent> IReadOnlyDictionary<Id, TComponent>.Values =>
        Components;

    bool IReadOnlyDictionary<Id, TComponent>.ContainsKey(Id key) =>
        Contains(key);

    bool IReadOnlyDictionary<Id, TComponent>.TryGetValue(Id key, out TComponent value) =>
        TryGet(key, out value);
}
