using System;

namespace Alitz.UnitTests;
public class EntityTests
{
    [Fact]
    public void MaxValuesArePositive()
    {
        Assert.True(Entity.MaxIndex > 0);
        Assert.True(Entity.MaxVersion > 0);
    }

    [Fact]
    public void MaxValuesAreGreaterThanMinValues()
    {
        Assert.True(Entity.MaxIndex > Entity.MinIndex);
        Assert.True(Entity.MaxVersion > Entity.MinVersion);
    }

    [Fact]
    public void IdIsConsistent() =>
        Assert.Equal(42, new Entity(42).Index);

    [Fact]
    public void VersionIsConsistent()
    {
        var entity = new Entity(42, 33);
        Assert.Equal(33, entity.Version);
    }

    [Fact]
    public void VersionIsZeroIfOmittedFromCtor()
    {
        Assert.Equal(0, new Entity().Version);
        Assert.Equal(0, new Entity(42).Version);
    }

    [Fact]
    public void CannotExceedMaxAndMinValuesOnCreation()
    {
        Assert.ThrowsAny<ArgumentOutOfRangeException>(() => _ = new Entity(Entity.MaxIndex + 1));
        Assert.ThrowsAny<ArgumentOutOfRangeException>(() => _ = new Entity(42, Entity.MaxVersion + 1));
        Assert.ThrowsAny<ArgumentOutOfRangeException>(() => _ = new Entity(Entity.MinIndex - 1));
        Assert.ThrowsAny<ArgumentOutOfRangeException>(() => _ = new Entity(42, Entity.MinVersion - 1));
    }

    [Fact]
    public void CreatingWithMaxValuesDoesNotThrow()
    {
        _ = new Entity(Entity.MaxIndex);
        _ = new Entity(42, Entity.MaxVersion);
    }

    [Fact]
    public void EqualityWorks()
    {
        var lhs = new Entity(42);
        // ReSharper disable once EqualExpressionComparison
        Assert.True(lhs.Equals(lhs));
        var rhs1 = new Entity(42);
        var rhs2 = new Entity(43);
        Assert.True(lhs.Equals(rhs1));
        Assert.False(lhs.Equals(rhs2));
    }

    [Fact]
    public void InequalityWorks()
    {
        var lhs = new Entity(42);
        var rhs1 = new Entity(42);
        var rhs2 = new Entity(43);
        Assert.False(!lhs.Equals(rhs1));
        Assert.True(!lhs.Equals(rhs2));
    }
}
