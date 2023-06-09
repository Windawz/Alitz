using System.Linq;

using Alitz.Collections;

namespace Alitz.UnitTests;
public class SparseSetTests
{
    public SparseSetTests()
    {
        _entitySpace = new EntitySpace();
        _set = new SparseSet<Entity>(IndexExtractor.Entity);
    }

    private readonly EntitySpace _entitySpace;
    private readonly SparseSet<Entity> _set;

    [Fact]
    public void EmptyCtor_CreatedEmpty() =>
        Assert.Multiple(() => Assert.Equal(0, _set.Count), () => Assert.Empty(_set.Values));

    [Fact]
    public void Contains_TrueForAddedValue()
    {
        var entity = _entitySpace.Create();
        _set.TryAdd(entity);
        Assert.True(_set.Contains(entity));
    }

    [Fact]
    public void Contains_FalseForNeverAddedValue() =>
        Assert.False(_set.Contains(_entitySpace.Create()));

    [Fact]
    public void Contains_FalseForRemovedValue()
    {
        var entity = _entitySpace.Create();
        _set.Add(entity);
        _set.Remove(entity);
        Assert.False(_set.Contains(entity));
    }

    [Fact]
    public void Remove_TrueIfContains()
    {
        var entity = _entitySpace.Create();
        _set.TryAdd(entity);
        Assert.True(_set.Remove(entity));
    }

    [Fact]
    public void Remove_FalseIfDoesNotContain() =>
        Assert.False(_set.Remove(_entitySpace.Create()));

    [Fact]
    public void Remove_CountDecreases()
    {
        var entity = _entitySpace.Create();
        var otherEntity = _entitySpace.Create();
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
        Assert.True(_set.TryAdd(_entitySpace.Create()));

    [Fact]
    public void TryAdd_FalseIfContains()
    {
        var entity = _entitySpace.Create();
        _set.TryAdd(entity);
        Assert.False(_set.TryAdd(entity));
    }

    [Fact]
    public void TryAdd_CountIncreases()
    {
        var entity = _entitySpace.Create();
        var otherEntity = _entitySpace.Create();
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
            _set.TryAdd(_entitySpace.Create());
        }
        SparseSet<Entity> emptySet = new(IndexExtractor.Entity);
        _set.Clear();
        Assert.Multiple(
            () => Assert.True(
                _set.Values.OrderBy(entity => entity.Id).SequenceEqual(emptySet.Values.OrderBy(entity => entity.Id))),
            () => Assert.Equal(emptySet.Count, _set.Count));
    }
}
