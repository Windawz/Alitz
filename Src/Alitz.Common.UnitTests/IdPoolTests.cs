using System.Collections.Generic;
using System.Linq;

using Alitz.Collections;

namespace Alitz.UnitTests;
public class IdPoolTests
{
    public IdPoolTests()
    {
        _pool = new IdPool();
    }

    private readonly IdPool _pool;

    [Fact]
    public void Fetch_FirstIdHasMinimumValues()
    {
        var id = _pool.Fetch();
        Assert.Equal(Id.MinIndex, id.Index);
        Assert.Equal(Id.MinVersion, id.Version);
    }

    [Fact]
    public void Fetch_SubsequentCallsWithoutStoringOnlyIncrementIndex()
    {
        for (int i = Id.MinIndex; i < Id.MinIndex + 100; i++)
        {
            var id = _pool.Fetch();
            Assert.Equal(i, id.Index);
            Assert.Equal(Id.MinVersion, id.Version);
        }
    }

    [Fact]
    public void Store_ReturnsTrueIfStoredIdWasFetchedAtTheMoment() =>
        Assert.True(_pool.Store(_pool.Fetch()));

    [Fact]
    public void Store_ReturnsFalseIfIdIsUnrelated() =>
        Assert.False(_pool.Store(new Id(42, 63)));

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
        Assert.False(_pool.IsOccupied(new Id(42, 63)));

    [Fact]
    public void IsOccupied_FalseForStoredId()
    {
        var id = _pool.Fetch();
        _pool.Store(id);
        Assert.False(_pool.IsOccupied(id));
    }

    [Fact]
    public void Occupied_EmptyInitially() =>
        Assert.Empty(_pool.Occupied);

    [Fact]
    public void Occupied_FetchIncrementsCount()
    {
        const int fetchCount = 100;
        int oldCount = _pool.Occupied.Count;
        for (int i = 0; i < fetchCount; i++)
        {
            _pool.Fetch();
            Assert.Equal(oldCount + 1, _pool.Occupied.Count);
            oldCount = _pool.Occupied.Count;
        }
    }

    public class OccupiedSingleTests
    {
        public OccupiedSingleTests()
        {
            _pool = new IdPool();
            _fetchedId = _pool.Fetch();
        }

        private readonly Id _fetchedId;

        private readonly IdPool _pool;

        [Fact]
        public void Occupied_ContainsFetchedId() =>
            Assert.Contains(_fetchedId, _pool.Occupied);

        [Fact]
        public void Occupied_DoesNotContainIdJustStored()
        {
            _pool.Store(_fetchedId);
            Assert.DoesNotContain(_fetchedId, _pool.Occupied);
        }
    }

    public class OccupiedManyTests
    {
        private const int InitialCount = 100;

        public OccupiedManyTests()
        {
            _pool = new IdPool();
            var fetchedIds = new List<Id>(InitialCount);
            for (int i = 0; i < InitialCount; i++)
            {
                fetchedIds.Add(_pool.Fetch());
            }
            _fetchedIds = fetchedIds;
        }

        private readonly IReadOnlyList<Id> _fetchedIds;

        private readonly IdPool _pool;

        [Fact]
        public void Occupied_StoreDecrementsCount()
        {
            int oldCount = _pool.Occupied.Count;
            foreach (var id in _fetchedIds)
            {
                _pool.Store(id);
                Assert.Equal(oldCount - 1, _pool.Occupied.Count);
                oldCount = _pool.Occupied.Count;
            }
        }

        [Fact]
        public void Occupied_ContainsArbitraryFetchedIds()
        {
            var arbitraryIds = _fetchedIds.Select((id, i) => (id, i))
                .Where(tuple => tuple.i % 4 == 0)
                .Select(tuple => tuple.id);

            Assert.All(arbitraryIds, id => Assert.Contains(id, _pool.Occupied));
        }
    }
}
