using System;
using System.Linq;

using Alitz.Collections;

namespace Alitz.UnitTests;
public class SparseColumnTests
{
    public SparseColumnTests()
    {
        _column = new SparseColumn<Component>();
        _entityFactory = new EntityFactory();
    }

    private readonly SparseColumn<Component> _column;
    private readonly EntityFactory _entityFactory;

    [Fact]
    public void EmptyCtor_EmptyOnCreation()
    {
        Assert.Empty(_column);
        Assert.Empty(_column.Entities);
        Assert.Empty(_column.Components);
    }

    [Fact]
    public void Indexer_Get_ThrowsIfKeyNotFound() =>
        Assert.Throws<ArgumentOutOfRangeException>(() => _column[_entityFactory.Create()]);

    [Fact]
    public void Indexer_Get_YieldsProperValueForKey()
    {
        var entity = _entityFactory.Create();
        Component value = new(42);
        _column.TryAdd(entity, value);
        Assert.Equal(value, _column[entity]);
    }

    [Fact]
    public void Indexer_Set_CreatesEntryIfKeyNotFound()
    {
        var entity = _entityFactory.Create();
        Component value = new(42);
        _column[entity] = value;
        Assert.True(_column.Contains(entity));
        Assert.Equal(value, _column[entity]);
    }

    [Fact]
    public void Indexer_Set_UpdatesEntryIfExists()
    {
        var entity = _entityFactory.Create();
        _column.TryAdd(entity, new Component(42));
        var newValue = new Component(63);
        _column[entity] = newValue;
        Assert.Equal(newValue, _column[entity]);
    }

    [Fact]
    public void TryAdd_IncreasesCount()
    {
        _column.TryAdd(_entityFactory.Create(), new Component(42));
        Assert.Single(_column);
    }

    [Fact]
    public void TryAdd_EntriesWithIdenticalKeysDisallowed()
    {
        var entity = _entityFactory.Create();
        var value1 = new Component(42);
        var value2 = new Component(63);
        Assert.True(_column.TryAdd(entity, value1));
        Assert.False(_column.TryAdd(entity, value2));
        Assert.Equal(value1, _column[entity]);
    }

    [Fact]
    public void TryAdd_TrueWhenNotFound() =>
        Assert.True(_column.TryAdd(_entityFactory.Create(), new Component(42)));

    [Fact]
    public void TryAdd_FalseWhenExists()
    {
        var entity = _entityFactory.Create();
        var value = new Component(42);
        _column.TryAdd(entity, value);
        Assert.False(_column.TryAdd(entity, value));
    }

    [Fact]
    public void Contains_TrueWhenExists()
    {
        var entity = _entityFactory.Create();
        _column.TryAdd(entity, new Component(42));
        Assert.True(_column.Contains(entity));
    }

    [Fact]
    public void Contains_FalseWhenNotFound() =>
        Assert.False(_column.Contains(_entityFactory.Create()));

    [Fact]
    public void Contains_FalseAfterRemoval()
    {
        var entity = _entityFactory.Create();
        _column.TryAdd(entity, new Component(42));
        _column.Remove(entity);
        Assert.False(_column.Contains(entity));
    }

    [Fact]
    public void Remove_DecreasesCount()
    {
        var entity = _entityFactory.Create();
        _column.TryAdd(entity, new Component(42));
        Assert.Single(_column);
        _column.Remove(entity);
        Assert.Empty(_column);
    }

    [Fact]
    public void Remove_TrueIfExisted()
    {
        var entity = _entityFactory.Create();
        _column.TryAdd(entity, new Component());
        Assert.True(_column.Remove(entity));
    }

    [Fact]
    public void Remove_FalseIfDidNotFind() =>
        Assert.False(_column.Remove(_entityFactory.Create()));

    [Fact]
    public void Remove_CannotGetAfter()
    {
        var entity = _entityFactory.Create();
        _column.TryAdd(entity, new Component(42));
        _column.Remove(entity);
        Assert.Throws<ArgumentOutOfRangeException>(() => _ = _column[entity]);
    }

    [Fact]
    public void Clear_SameAsEmptyInstance()
    {
        foreach (int i in Enumerable.Range(0, 5))
        {
            _column.TryAdd(_entityFactory.Create(), new Component(i));
        }
        _column.Clear();
        var emptyColumn = new SparseColumn<Component>();
        Assert.Equal(emptyColumn.Count, _column.Count);
        Assert.True(
            _column.Entities.OrderBy(entity => entity.Index)
                .SequenceEqual(emptyColumn.Entities.OrderBy(entity => entity)));
        Assert.True(
            _column.Components.OrderBy(component => component.Value)
                .SequenceEqual(emptyColumn.Components.OrderBy(component => component.Value)));
    }

    [Fact]
    public void TryGet_TrueIfExists()
    {
        var entity = _entityFactory.Create();
        _column.TryAdd(entity, new Component(42));
        Assert.True(_column.TryGet(entity, out _));
    }

    [Fact]
    public void TryGet_FalseIfNotFound() =>
        Assert.False(_column.TryGet(_entityFactory.Create(), out _));

    [Fact]
    public void TryGet_ResultEqualsJustAdded()
    {
        var entity = _entityFactory.Create();
        var component = new Component(42);
        _column.TryAdd(entity, component);
        _column.TryGet(entity, out var result);
        Assert.Equal(component, result);
    }

    [Fact]
    public void TrySet_TrueIfExists()
    {
        var entity = _entityFactory.Create();
        _column.TryAdd(entity, new Component(42));
        Assert.True(_column.TrySet(entity, new Component(63)));
    }

    [Fact]
    public void TrySet_FalseIfNotFound() =>
        Assert.False(_column.TrySet(_entityFactory.Create(), new Component(42)));

    [Fact]
    public void TrySet_ValueChangesIfTrue()
    {
        var entity = _entityFactory.Create();
        _column.TryAdd(entity, new Component(42));
        var newComponent = new Component(63);
        Assert.True(_column.TrySet(entity, newComponent));
        Assert.Equal(newComponent, _column[entity]);
    }

    [Fact]
    public void GetByRef_ThrowsIfNotFound() =>
        Assert.Throws<ArgumentOutOfRangeException>(() => _column.GetByRef(_entityFactory.Create()));

    [Fact]
    public void GetByRef_MutableStructChanges()
    {
        var entity = _entityFactory.Create();
        _column.TryAdd(entity, new Component(42));
        ref var component = ref _column.GetByRef(entity);
        component.Value = 63;
        Assert.Equal(63, _column[entity].Value);
    }

    private record struct Component(float Value);

    private class EntityFactory
    {
        private Entity current = new(Entity.MinIndex, Entity.MinVersion);

        public Entity Create()
        {
            var result = current;
            current = new Entity(current.Index + 1, current.Version);
            return result;
        }
    }
}
