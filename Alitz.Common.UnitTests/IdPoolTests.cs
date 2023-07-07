using Alitz.Collections;

namespace Alitz.UnitTests;
public class IdPoolTests
{
    public IdPoolTests()
    {
        _pool = new IdPool<GenericId>();
    }

    private readonly IdPool<GenericId> _pool;

    [Fact]
    public void Fetch_FirstIdHasMinimumValues()
    {
        var id = _pool.Fetch();
        Assert.Equal(GenericId.MinIndex, id.Index);
        Assert.Equal(GenericId.MinVersion, id.Version);
    }

    [Fact]
    public void Fetch_SubsequentCallsWithoutStoringOnlyIncrementIndex()
    {
        for (int i = GenericId.MinIndex; i < GenericId.MinIndex + 100; i++)
        {
            var id = _pool.Fetch();
            Assert.Equal(i, id.Index);
            Assert.Equal(GenericId.MinVersion, id.Version);
        }
    }

    [Fact]
    public void Store_ReturnsTrueIfStoredIdWasFetchedAtTheMoment() =>
        Assert.True(_pool.Store(_pool.Fetch()));

    [Fact]
    public void Store_ReturnsFalseIfIdIsUnrelated() =>
        Assert.False(_pool.Store(new GenericId(42, 63)));

    [Fact]
    public void Store_ReturnsFalseIfIdHasAlreadyBeenSuccessfullyStored()
    {
        var id = _pool.Fetch();
        _pool.Store(id);
        Assert.False(_pool.Store(id));
    }

    [Fact]
    public void Store_SubsequentFetchCallReusesIndexAndIncrementsVersion()
    {
        var oldId = _pool.Fetch();
        int oldIndex = oldId.Index;
        int oldVersion = oldId.Version;
        _pool.Store(oldId);
        var newId = _pool.Fetch();
        Assert.Equal(oldIndex, newId.Index);
        Assert.Equal(oldVersion + 1, newId.Version);
    }

    [Fact]
    public void IsOccupied_TrueForFetchedId() =>
        Assert.True(_pool.IsOccupied(_pool.Fetch()));

    [Fact]
    public void IsOccupied_FalseForUnrelatedId() =>
        Assert.False(_pool.IsOccupied(new GenericId(42, 63)));

    [Fact]
    public void IsOccupied_FalseForStoredId()
    {
        var id = _pool.Fetch();
        _pool.Store(id);
        Assert.False(_pool.IsOccupied(id));
    }
}
