using System;

namespace Alitz.UnitTests;
public class IdTests
{
    [Fact]
    public void Limits_MaxValuesAreGreaterThanMinValues()
    {
        Assert.True(Id.MaxIndex > Id.MinIndex);
        Assert.True(Id.MaxVersion > Id.MinVersion);
    }

    [Fact]
    public void Limits_AreNotNegative()
    {
        Assert.True(Id.MinIndex > 0);
        Assert.True(Id.MinVersion > 0);
        Assert.True(Id.MaxIndex > 0);
        Assert.True(Id.MaxVersion > 0);
    }

    [Fact]
    public void Index_EqualToConstructorArgument() =>
        Assert.Equal(42, new Id(42, Id.MinVersion).Index);

    [Fact]
    public void Version_EqualToConstructorArgument()
    {
        var id = new Id(Id.MinIndex, 42);
        Assert.Equal(42, id.Version);
    }

    [Fact]
    public void Limits_Constructor_ThrowsIfLimitsAreExceeded()
    {
        Assert.ThrowsAny<ArgumentOutOfRangeException>(() => _ = new Id(Id.MaxIndex + 1, 42));
        Assert.ThrowsAny<ArgumentOutOfRangeException>(() => _ = new Id(42, Id.MaxVersion + 1));
        Assert.ThrowsAny<ArgumentOutOfRangeException>(() => _ = new Id(Id.MinIndex - 1, 42));
        Assert.ThrowsAny<ArgumentOutOfRangeException>(() => _ = new Id(42, Id.MinVersion - 1));
    }

    [Fact]
    public void Limits_Constructor_DoesNotFailWhenCalledWithMinAndMaxValues()
    {
        _ = new Id(Id.MinIndex, 42);
        _ = new Id(Id.MaxIndex, 42);
        _ = new Id(42, Id.MinVersion);
        _ = new Id(42, Id.MaxVersion);
        _ = new Id(Id.MinIndex, Id.MinVersion);
        _ = new Id(Id.MinIndex, Id.MaxVersion);
        _ = new Id(Id.MaxIndex, Id.MinVersion);
        _ = new Id(Id.MaxIndex, Id.MaxVersion);
    }

    [Fact]
    public void Equals_TrueIfAndOnlyIfBothIndicesAndVersionsAreEqual()
    {
        Assert.NotEqual(new Id(42, 42), new Id(42, 63));
        Assert.NotEqual(new Id(42, 42), new Id(63, 42));
        Assert.NotEqual(new Id(42, 42), new Id(63, 63));
        Assert.Equal(new Id(42, 42), new Id(42, 42));
    }
}
