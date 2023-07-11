using System;

namespace Alitz.UnitTests;
public abstract class IdTests<TId> : IdPreparationTests<TId> where TId : struct, IId<TId>
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
    public void Limits_MaxValuesAreGreaterThanMinValues()
    {
        Assert.True(_maxIndex > _minIndex);
        Assert.True(_maxVersion > _minVersion);
    }

    [Fact]
    public void Limits_AreNotNegative()
    {
        Assert.True(_minIndex > 0);
        Assert.True(_minVersion > 0);
        Assert.True(_maxIndex > 0);
        Assert.True(_maxVersion > 0);
    }

    [Fact]
    public void Index_EqualToConstructorArgument() =>
        Assert.Equal(42, _constructor(42, _minVersion).Index);

    [Fact]
    public void Version_EqualToConstructorArgument()
    {
        var id = _constructor(_minIndex, 42);
        Assert.Equal(42, id.Version);
    }

    [Fact]
    public void Limits_Constructor_ThrowsIfLimitsAreExceeded()
    {
        Assert.ThrowsAny<ArgumentOutOfRangeException>(() => _ = _constructor(_maxIndex + 1, 42));
        Assert.ThrowsAny<ArgumentOutOfRangeException>(() => _ = _constructor(42, _maxVersion + 1));
        Assert.ThrowsAny<ArgumentOutOfRangeException>(() => _ = _constructor(_minIndex - 1, 42));
        Assert.ThrowsAny<ArgumentOutOfRangeException>(() => _ = _constructor(42, _minVersion - 1));
    }

    [Fact]
    public void Limits_Constructor_DoesNotFailWhenCalledWithMinAndMaxValues()
    {
        _constructor(_minIndex, 42);
        _constructor(_maxIndex, 42);
        _constructor(42, _minVersion);
        _constructor(42, _maxVersion);
        _constructor(_minIndex, _minVersion);
        _constructor(_minIndex, _maxVersion);
        _constructor(_maxIndex, _minVersion);
        _constructor(_maxIndex, _maxVersion);
    }

    [Fact]
    public void Equals_TrueIfAndOnlyIfBothIndicesAndVersionsAreEqual()
    {
        Assert.NotEqual(_constructor(42, 42), _constructor(42, 63));
        Assert.NotEqual(_constructor(42, 42), _constructor(63, 42));
        Assert.NotEqual(_constructor(42, 42), _constructor(63, 63));
        Assert.Equal(_constructor(42, 42), _constructor(42, 42));
    }
}
