using System;

namespace Alitz.Ecs.UnitTests; 
public class EntityTests {
    [Fact]
    public void MaxValuesArePositive() {
        Assert.True(Entity.IdMaxValue > 0);
        Assert.True(Entity.VersionMaxValue > 0);
    }
    
    [Fact]
    public void MaxValuesAreGreaterThanMinValues() {
        Assert.True(Entity.IdMaxValue > Entity.IdMinValue);
        Assert.True(Entity.VersionMaxValue > Entity.VersionMinValue);
    }
    
    [Fact]
    public void DefaultConstructorReturnsNullEntity() {
        Assert.Equal(Entity.Null, new Entity());
    }
    
    [Fact]
    public void IdIsConsistent() {
        Assert.Equal(42, new Entity(42).Id);
        Assert.Equal(Entity.Null.Id, new Entity().Id);
    }
    
    [Fact]
    public void VersionIsConsistent() {
        var entity = new Entity(42, 33);
        Assert.Equal(33, entity.Version);
    }
    
    [Fact]
    public void VersionIsZeroIfOmittedFromCtor() {
        Assert.Equal(0, new Entity().Version);
        Assert.Equal(0, new Entity(42).Version);
    }
    
    [Fact]
    public void CannotCreateEntityWithIdOfNullEntity() {
        Assert.ThrowsAny<ArgumentOutOfRangeException>(() => new Entity(Entity.Null.Id));
    }
    
    [Fact]
    public void CannotExceedMaxAndMinValuesOnCreation() {
        Assert.ThrowsAny<ArgumentOutOfRangeException>(() => _ = new Entity(Entity.IdMaxValue + 1));
        Assert.ThrowsAny<ArgumentOutOfRangeException>(() => _ = new Entity(42, Entity.VersionMaxValue + 1));
        Assert.ThrowsAny<ArgumentOutOfRangeException>(() => _ = new Entity(Entity.IdMinValue - 1));
        Assert.ThrowsAny<ArgumentOutOfRangeException>(() => _ = new Entity(42, Entity.VersionMinValue - 1));
    }
    
    [Fact]
    public void CreatingWithMaxValuesDoesNotThrow() {
        _ = new Entity(Entity.IdMaxValue);
        _ = new Entity(42, Entity.VersionMaxValue);
    }
    
    [Fact]
    public void EqualityWorks() {
        var lhs = new Entity(42);
        // ReSharper disable once EqualExpressionComparison
        Assert.True(lhs == lhs);
        var rhs1 = new Entity(42);
        var rhs2 = new Entity(43);
        Assert.True(lhs == rhs1);
        Assert.False(lhs == rhs2);
    }
    
    [Fact]
    public void InequalityWorks() {
        var lhs = new Entity(42);
        var rhs1 = new Entity(42);
        var rhs2 = new Entity(43);
        Assert.False(lhs != rhs1);
        Assert.True(lhs != rhs2);
    }
    
    [Fact]
    public void EqualsMethodMatchesOperatorBehavior() {
        var lhs = new Entity(42);
        var rhs1 = new Entity(42);
        var rhs2 = new Entity(43);
        Assert.Equal(
            lhs == rhs1,
            lhs.Equals(rhs1)
        );
        Assert.Equal(
            lhs == rhs2,
            lhs.Equals(rhs2)
        );
        Assert.Equal(
            lhs != rhs1,
            !lhs.Equals(rhs1)
        );
        Assert.Equal(
            lhs != rhs2,
            !lhs.Equals(rhs2)
        );
    }
    
    [Fact]
    public void ObjectEqualityWorks() {
        var lhs = new Entity(42);
        object rhs1 = new Entity(42);
        object rhs2 = new Entity(43);
        Assert.True(lhs.Equals(rhs1));
        Assert.False(lhs.Equals(rhs2));
        Assert.False(lhs.Equals(null));
        // ReSharper disable once SuspiciousTypeConversion.Global
        Assert.False(lhs.Equals("Hello world!"));
    }
    
    [Fact]
    public void NullEntityHashCodeMatchesItsId() {
        Assert.Equal(Entity.Null.Id, Entity.Null.GetHashCode());
    }
    
    [Fact]
    public void NonNullEntityHashCodeIsNotEqualToNullEntityHashCode() {
        Assert.NotEqual(Entity.Null.GetHashCode(), new Entity(42).GetHashCode());
    }
}
