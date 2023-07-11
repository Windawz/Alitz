namespace Alitz.UnitTests;
public abstract class IdPreparationTests<TId> where TId : struct, IId<TId>
{
    [Fact]
    public void Constructor_Present() =>
        Assert.True(Id.DiscoverConstructor<TId>() is not null);

    [Fact]
    public void Limits_Present() =>
        Assert.True(Id.DiscoverLimits<TId>() is not null);
}
