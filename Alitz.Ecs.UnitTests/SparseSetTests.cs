using System.Linq;

using Alitz.Collections;

namespace Alitz.UnitTests;
public class SparseSetTests
{
    public SparseSetTests()
    {
        _space = new EntitySpace();
        _set = new SparseSet<Entity>(IndexExtractor.Entity);
    }

    private readonly SparseSet<Entity> _set;

    private readonly EntitySpace _space;

    [Fact]
    public void EmptyCtor_CreatedEmpty() =>
        Assert.Multiple(() => Assert.Equal(0, _set.Count), () => Assert.Empty(_set.Values));

    [Fact]
    public void Contains_TrueForAddedValue()
    {
        var entity = _space.Create();
        _set.TryAdd(entity);
        Assert.True(_set.Contains(entity));
    }

    [Fact]
    public void Contains_FalseForNeverAddedValue() =>
        Assert.False(_set.Contains(_space.Create()));

    [Fact]
    public void Contains_FalseForRemovedValue()
    {
        var entity = _space.Create();
        _set.Add(entity);
        _set.Remove(entity);
        Assert.False(_set.Contains(entity));
    }

    [Fact]
    public void Remove_TrueIfContains()
    {
        var entity = _space.Create();
        _set.TryAdd(entity);
        Assert.True(_set.Remove(entity));
    }

    [Fact]
    public void Remove_FalseIfDoesNotContain() =>
        Assert.False(_set.Remove(_space.Create()));

    [Fact]
    public void Remove_CountDecreases()
    {
        var entity = _space.Create();
        var otherEntity = _space.Create();
        _set.Add(entity);
        _set.Add(otherEntity);
        Assert.Equal(2, _set.Count);
        _set.Remove(entity);
        Assert.Equal(1, _set.Count);
        _set.Remove(otherEntity);
        Assert.Equal(0, _set.Count);
    }

    [Fact]
    public void TryAdd_TrueIfDoesNotContain() =>
        Assert.True(_set.TryAdd(_space.Create()));

    [Fact]
    public void TryAdd_FalseIfContains()
    {
        var entity = _space.Create();
        _set.TryAdd(entity);
        Assert.False(_set.TryAdd(entity));
    }

    [Fact]
    public void TryAdd_CountIncreases()
    {
        var entity = _space.Create();
        var otherEntity = _space.Create();
        Assert.Equal(0, _set.Count);
        _set.TryAdd(entity);
        Assert.Equal(1, _set.Count);
        _set.TryAdd(otherEntity);
        Assert.Equal(2, _set.Count);
    }

    [Fact]
    public void Clear_ResultIdenticalToEmptyInstance()
    {
        foreach (int _ in Enumerable.Range(0, 5))
        {
            _set.TryAdd(_space.Create());
        }
        SparseSet<Entity> emptySet = new(IndexExtractor.Entity);
        _set.Clear();
        Assert.Multiple(
            () => Assert.True(
                _set.Values.OrderBy(entity => entity.Id).SequenceEqual(emptySet.Values.OrderBy(entity => entity.Id))),
            () => Assert.Equal(emptySet.Count, _set.Count));
    }

    public class EntityVersioningTests
    {
        public EntityVersioningTests()
        {
            _set = new SparseSet<Entity>(IndexExtractor.Entity);
            var startEntity = new Entity(Entity.IdMinValue, Entity.VersionMinValue);
            _set.Add(startEntity);
        }

        private readonly SparseSet<Entity> _set;

        private Entity StartEntity =>
            _set.Values.First();

        [Fact]
        public void TryAdd_TrueIfOnlyVersionDiffers() =>
            Assert.True(_set.TryAdd(NextVersionOf(StartEntity)));

        [Fact]
        public void Contains_FalseIfVersionDiffers() =>
            Assert.False(_set.Contains(NextVersionOf(StartEntity)));

        [Fact]
        public void Remove_FalseIfVersionDiffers() =>
            Assert.False(_set.Remove(NextVersionOf(StartEntity)));

        private static Entity NextVersionOf(Entity entity) =>
            new(entity.Id, entity.Version + 1);
    }
}
