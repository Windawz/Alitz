using System;

namespace Alitz.UnitTests;
public class GenericIdTests
{
    [Fact]
    public void MaxValuesAreGreaterThanMinValues()
    {
        Assert.True(GenericId.MaxIndex > GenericId.MinIndex);
        Assert.True(GenericId.MaxVersion > GenericId.MinVersion);
    }

    [Fact]
    public void IdIsConsistent() =>
        Assert.Equal(42, new GenericId(42).Index);

    [Fact]
    public void VersionIsConsistent()
    {
        var id = new GenericId(42, 33);
        Assert.Equal(33, id.Version);
    }

    [Fact]
    public void VersionIsZeroIfOmittedFromCtor()
    {
        Assert.Equal(0, new GenericId().Version);
        Assert.Equal(0, new GenericId(42).Version);
    }

    [Fact]
    public void CannotExceedMaxAndMinValuesOnCreation()
    {
        Assert.ThrowsAny<ArgumentOutOfRangeException>(() => _ = new GenericId(GenericId.MaxIndex + 1));
        Assert.ThrowsAny<ArgumentOutOfRangeException>(() => _ = new GenericId(42, GenericId.MaxVersion + 1));
        Assert.ThrowsAny<ArgumentOutOfRangeException>(() => _ = new GenericId(GenericId.MinIndex - 1));
        Assert.ThrowsAny<ArgumentOutOfRangeException>(() => _ = new GenericId(42, GenericId.MinVersion - 1));
    }

    [Fact]
    public void CreatingWithMaxValuesDoesNotThrow()
    {
        _ = new GenericId(GenericId.MaxIndex);
        _ = new GenericId(42, GenericId.MaxVersion);
    }

    [Fact]
    public void EqualityWorks()
    {
        var lhs = new GenericId(42);
        // ReSharper disable once EqualExpressionComparison
        Assert.True(lhs.Equals(lhs));
        var rhs1 = new GenericId(42);
        var rhs2 = new GenericId(43);
        Assert.True(lhs.Equals(rhs1));
        Assert.False(lhs.Equals(rhs2));
    }

    [Fact]
    public void InequalityWorks()
    {
        var lhs = new GenericId(42);
        var rhs1 = new GenericId(42);
        var rhs2 = new GenericId(43);
        Assert.False(!lhs.Equals(rhs1));
        Assert.True(!lhs.Equals(rhs2));
    }
}
