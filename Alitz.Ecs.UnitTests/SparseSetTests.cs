using Alitz.Ecs.Collections;

namespace Alitz.Ecs.UnitTests;
public class SparseSetTests
{
    public SparseSetTests()
    {
        _set = new SparseSet<int>(IndexExtractors.Int32IndexExtractor);
    }

    private readonly SparseSet<int> _set;

    [Fact]
    public void Contains_TrueForAddedValue()
    {
        _set.TryAdd(42);
        Assert.True(_set.Contains(42));
    }

    [Fact]
    public void Contains_FalseForNeverAddedValue() =>
        Assert.False(_set.Contains(42));

    [Fact]
    public void Contains_FalseForRemovedValue()
    {
        _set.Add(42);
        _set.Remove(42);
        Assert.False(_set.Contains(42));
    }

    [Fact]
    public void Remove_TrueIfContains()
    {
        _set.TryAdd(42);
        Assert.True(_set.Remove(42));
    }

    [Fact]
    public void Remove_FalseIfDoesNotContain() =>
        Assert.False(_set.Remove(42));

    [Fact]
    public void Remove_CountDecreases()
    {
        _set.Add(42);
        _set.Add(63);
        Assert.Equal(2, _set.Count);
        _set.Remove(42);
        Assert.Equal(1, _set.Count);
        _set.Remove(63);
        Assert.Equal(0, _set.Count);
    }

    [Fact]
    public void TryAdd_TrueIfDoesNotContain() =>
        Assert.True(_set.TryAdd(42));

    [Fact]
    public void TryAdd_FalseIfContains()
    {
        _set.TryAdd(42);
        Assert.False(_set.TryAdd(42));
    }

    [Fact]
    public void TryAdd_CountIncreases()
    {
        Assert.Equal(0, _set.Count);
        _set.TryAdd(42);
        Assert.Equal(1, _set.Count);
        _set.TryAdd(63);
        Assert.Equal(2, _set.Count);
    }
}
