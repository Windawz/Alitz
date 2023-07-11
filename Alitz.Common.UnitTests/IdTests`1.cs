using System;

namespace Alitz.UnitTests;
public abstract class IdTests<TId> : IdConstructorAndLimitTests<TId> where TId : struct, IId<TId>
{
    public IdTests()
    {
        _constructor = Id.DiscoverConstructor<TId>()
            ?? throw new InvalidOperationException($"{typeof(TId)} does not have the required constructor");
        (_minIndex, _minVersion, _maxIndex, _maxVersion) = Id.DiscoverLimits<TId>()
            ?? throw new InvalidOperationException(
                $"{typeof(TId)} does not have the required public static limit members");
    }

    private readonly Id.Constructor<TId> _constructor;
    private readonly int _maxIndex;
    private readonly int _maxVersion;
    private readonly int _minIndex;
    private readonly int _minVersion;

    [Fact]
    public void MaxValuesAreGreaterThanMinValues()
    {
        Assert.True(_maxIndex > _minIndex);
        Assert.True(_maxVersion > _minVersion);
    }

    [Fact]
    public void IdIsConsistent() =>
        Assert.Equal(42, _constructor(42, _minVersion).Index);

    [Fact]
    public void VersionIsConsistent()
    {
        var id = _constructor(_minIndex, 42);
        Assert.Equal(42, id.Version);
    }

    [Fact]
    public void CannotExceedMaxAndMinValuesOnCreation()
    {
        Assert.ThrowsAny<ArgumentOutOfRangeException>(() => _ = _constructor(_maxIndex + 1, 42));
        Assert.ThrowsAny<ArgumentOutOfRangeException>(() => _ = _constructor(42, _maxVersion + 1));
        Assert.ThrowsAny<ArgumentOutOfRangeException>(() => _ = _constructor(_minIndex - 1, 42));
        Assert.ThrowsAny<ArgumentOutOfRangeException>(() => _ = _constructor(42, _minVersion - 1));
    }

    [Fact]
    public void CreatingWithMaxValuesDoesNotThrow()
    {
        _ = _constructor(_maxIndex, 42);
        _ = _constructor(42, _maxVersion);
    }

    [Fact]
    public void EqualityWorks()
    {
        var lhs = _constructor(42, _minVersion);
        // ReSharper disable once EqualExpressionComparison
        Assert.True(lhs.Equals(lhs));
        var rhs1 = _constructor(42, _minVersion);
        var rhs2 = _constructor(43, _minVersion);
        Assert.True(lhs.Equals(rhs1));
        Assert.False(lhs.Equals(rhs2));
    }

    [Fact]
    public void InequalityWorks()
    {
        var lhs = _constructor(42, _minVersion);
        var rhs1 = _constructor(42, _minVersion);
        var rhs2 = _constructor(43, _minVersion);
        Assert.False(!lhs.Equals(rhs1));
        Assert.True(!lhs.Equals(rhs2));
    }
}
