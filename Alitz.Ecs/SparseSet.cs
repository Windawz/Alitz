using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Alitz.Ecs;
public class SparseSet<TComponent> where TComponent : struct {
    private const int NullIndex = -1;

    public SparseSet() {
        _sparse = new List<int>();
        _dense = new List<Entity>();
        _components = new List<TComponent>();
    }

    private readonly List<TComponent> _components;
    private readonly List<Entity> _dense;
    private readonly List<int> _sparse;
    
    public IEnumerable<Entity> Entities {
        get {
            foreach (Entity entity in _dense) {
                yield return entity;
            }
        }
    }
    
    public IEnumerable<TComponent> Components {
        get {
            foreach(TComponent component in _components) {
                yield return component;
            }
        }
    }

    public TComponent this[Entity entity] {
        get {
            (bool contains, _, int entityIndex) = RichContains(entity);
            if (!contains) {
                throw new ArgumentOutOfRangeException(nameof(entity));
            }
            return _components[entityIndex];
        }
        set {
            (bool contains, bool isInRange, int entityIndex) = RichContains(entity);
            if (!contains && !isInRange) {
                ResizeSparseList(entity.Id + 1);
                entityIndex = _sparse[entity.Id];
            }
            if (entityIndex == NullIndex) {
                entityIndex = _sparse[entity.Id] = _dense.Count;
                _dense.Add(entity);
                _components.Add(new TComponent());
            }
            _components[entityIndex] = value;
        }
    }
    
    public ref TComponent GetByRef(Entity entity) {
        (bool contains, _, int entityIndex) = RichContains(entity);
        if (!contains) {
            throw new ArgumentOutOfRangeException(nameof(entity));
        }
        return ref CollectionsMarshal.AsSpan(_components)[entityIndex];
    }
    
    public bool Contains(Entity entity) =>
        RichContains(entity).contains;
    
    public bool Remove(Entity entity) {
        (bool contains, _, int entityIndex) = RichContains(entity);
        if (!contains) {
            return false;
        }
        _sparse[entity.Id] = NullIndex;
        _dense.RemoveAt(entityIndex);
        _components.RemoveAt(entityIndex);
        return true;
    }
    
    private (bool contains, bool isInRange, int entityIndex) RichContains(Entity entity) {
        bool isInRange = entity.Id < _sparse.Count;
        int entityIndex = isInRange ? _sparse[entity.Id] : NullIndex;
        bool contains = isInRange && entityIndex != NullIndex;
        return (contains, isInRange, entityIndex);
    }

    private void ResizeSparseList(int count) {
        if (count < 0) {
            throw new ArgumentOutOfRangeException(nameof(count));
        }
        int currentCount = _sparse.Count;
        if (count < currentCount) {
            _sparse.RemoveRange(count, currentCount - count);
        } else {
            _sparse.EnsureCapacity(count);
            _sparse.AddRange(Enumerable.Repeat(NullIndex, count - currentCount));
        }
    }
}
