using System.Collections.Generic;

namespace Alitz.Collections;
public partial class SparseColumn<TComponent>
{
    IEnumerable<Entity> IReadOnlyDictionary<Entity, TComponent>.Keys =>
        Entities;

    IEnumerable<TComponent> IReadOnlyDictionary<Entity, TComponent>.Values =>
        Components;

    bool IReadOnlyDictionary<Entity, TComponent>.ContainsKey(Entity key) =>
        Contains(key);

    bool IReadOnlyDictionary<Entity, TComponent>.TryGetValue(Entity key, out TComponent value) =>
        TryGet(key, out value);
}
