using System;
using System.Linq;

namespace Alitz.Ecs.UnitTests;
public class SparseSetTests {
    [Fact]
    public void ComponentGetsAddedProperly() {
        var set = PrepareSet(0, v => new ImmutableComponent(v));
        Assert.Empty(set.Entities);
        Assert.Empty(set.Components);
        var entity = new Entity(42);
        var oldComponent = new ImmutableComponent(42);
        set[entity] = oldComponent;
        Assert.Single(set.Entities);
        Assert.Single(set.Components);
        var component = set[entity];
        Assert.Equal(oldComponent.Value, component.Value);
    }

    [Fact]
    public void ComponentGotByReferenceMutatesProperly() {
        var set = PrepareSet(3, v => new MutableComponent(v));
        const int value = 42;
        var entity = new Entity(0);
        ref var component = ref set.GetByRef(entity);
        component.Value = value;
        Assert.Equal(value, set[entity].Value);
    }

    [Fact]
    public void IndexingWithNullEntityThrowsNullEntityException() {
        var set = PrepareSet(3, v => new ImmutableComponent(v));
        Assert.ThrowsAny<NullEntityException>(() => _ = set[Entity.Null]);
        Assert.ThrowsAny<NullEntityException>(() => set.GetByRef(Entity.Null));
        Assert.ThrowsAny<NullEntityException>(() => set[Entity.Null] = new ImmutableComponent(0));
    }

    [Fact]
    public void ComponentOfRemovedEntityGetsRemovedProperly() {
        var set = PrepareSet(3, v => new ImmutableComponent(v));
        int oldEntityCount = set.Entities.Count();
        int oldComponentCount = set.Components.Count();
        var firstEntity = set.Entities.First();
        bool removed = set.Remove(firstEntity);
        Assert.True(removed);
        Assert.ThrowsAny<ArgumentOutOfRangeException>(() => _ = set[firstEntity]);
        Assert.Equal(oldEntityCount - 1, set.Entities.Count());
        Assert.Equal(oldComponentCount - 1, set.Components.Count());
    }

    private static SparseSet<TComponent> PrepareSet<TComponent>(int count, Func<int, TComponent> factory)
        where TComponent : struct {
        var set = new SparseSet<TComponent>();
        for (int i = 0; i < count; i++) {
            var entity = new Entity(i);
            var component = factory(Random.Shared.Next(-500, 501));
            set[entity] = component;
        }
        return set;
    }

    private struct MutableComponent {
        public MutableComponent(int value) {
            Value = value;
        }

        public int Value { get; set; }
    }

    private readonly struct ImmutableComponent {
        public ImmutableComponent(int value) {
            Value = value;
        }

        public int Value { get; }
    }
}
