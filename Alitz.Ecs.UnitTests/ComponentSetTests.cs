using System;
using System.Linq;

namespace Alitz.Ecs.UnitTests;
public class SparseSetTests {
    [Fact]
    public void SetCreated() {
        _ = new SparseSet<ImmutableComponent>();
    }
    
    [Fact]
    public void ComponentAdded() {
        var set = PrepareSet<ImmutableComponent>();
        Assert.Equal(0, set.Count);
        set[new Entity(42)] = new ImmutableComponent(42);
        Assert.Equal(1, set.Count);
    }
    
    [Fact]
    public void ComponentGot() {
        var set = PrepareSet<ImmutableComponent>(1);
        var entity = set.Entities.First();
        _ = set[entity];
    }
    
    [Fact]
    public void ComponentSet() {
        var set = PrepareSet<ImmutableComponent>(1);
        var entity = set.Entities.First();
        int oldValue = set[entity].Value;
        set[entity] = new ImmutableComponent(42);
        Assert.NotEqual(oldValue, set[entity].Value);
    }
    
    [Fact]
    public void ComponentGotAndSetByRef() {
        var set = PrepareSet<MutableComponent>(1);
        var entity = set.Entities.First();
        ref var component = ref set.GetByRef(entity);
        component.Value = 42;
        Assert.Equal(42, set[entity].Value);
    }
    
    [Fact]
    public void ComponentRemoved() {
        var set = PrepareSet<ImmutableComponent>(1);
        var entity = set.Entities.First();
        Assert.True(set.Remove(entity));
        Assert.Equal(0, set.Count);
        Assert.ThrowsAny<ArgumentOutOfRangeException>(() => set[entity]);
    }
    
    [Fact]
    public void CountIsConsistent() {
        var set = PrepareSet<ImmutableComponent>();
        Assert.Equal(0, set.Count);
        Assert.Equal(set.Count, set.Entities.Count());
        Assert.Equal(set.Count, set.Components.Count());
        var entity = new Entity(42);
        set[entity] = new ImmutableComponent(42);
        Assert.Equal(1, set.Count);
        Assert.Equal(set.Count, set.Entities.Count());
        Assert.Equal(set.Count, set.Components.Count());
        set.Remove(entity);
        Assert.Equal(0, set.Count);
        Assert.Equal(set.Count, set.Entities.Count());
        Assert.Equal(set.Count, set.Components.Count());
    }
    
    private static SparseSet<TComponent> PrepareSet<TComponent>(int count = 0, Func<int, TComponent>? factory = null)
        where TComponent : struct {
        factory ??= _ => new TComponent();
        var set = new SparseSet<TComponent>();
        for (int i = 0; i < count; i++) {
            var entity = new Entity(i);
            var component = factory(Random.Shared.Next());
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
