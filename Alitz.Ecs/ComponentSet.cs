using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Alitz.Ecs;
public sealed class ComponentSet<TComponent> : SparseSet where TComponent : struct {
    public ComponentSet() {
        _components = new List<TComponent>();
    }

    private readonly List<TComponent> _components;
    
    public IEnumerable<TComponent> Components {
        get {
            foreach(TComponent component in _components) {
                yield return component;
            }
        }
    }

    public TComponent this[Entity entity] {
        get {
            (bool contains, _, int entityIndex) = SparseListRichContains(entity);
            if (!contains) {
                throw new ArgumentOutOfRangeException(nameof(entity));
            }
            return _components[entityIndex];
        }
        set {
            int entityIndex = TryAddEntity(entity);
            _components[entityIndex] = value;
        }
    }
    
    public ref TComponent GetByRef(Entity entity) {
        (bool contains, _, int entityIndex) = SparseListRichContains(entity);
        if (!contains) {
            throw new ArgumentOutOfRangeException(nameof(entity));
        }
        return ref CollectionsMarshal.AsSpan(_components)[entityIndex];
    }
    
    public bool Remove(Entity entity) {
        int entityIndex = TryRemoveEntity(entity);
        if (entityIndex == InvalidDenseListIndex) {
            return false;
        }
        _components.RemoveAt(entityIndex);
        return true;
    }
}
