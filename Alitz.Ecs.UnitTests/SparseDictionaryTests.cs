using System;
using System.Linq;

namespace Alitz.UnitTests;
public class SparseDictionaryTests
{
    public SparseDictionaryTests()
    {
        _dictionary = new SparseDictionary<Entity, Component>(IndexExtractor.Entity);
        _entitySpace = new EntitySpace();
    }

    private readonly SparseDictionary<Entity, Component> _dictionary;
    private readonly EntitySpace _entitySpace;

    [Fact]
    public void EmptyCtor_EmptyOnCreation()
    {
        Assert.Equal(0, _dictionary.Count);
        Assert.Empty(_dictionary.Keys);
        Assert.Empty(_dictionary.Values);
    }

    [Fact]
    public void Indexer_Get_ThrowsIfKeyNotFound() =>
        Assert.Throws<ArgumentOutOfRangeException>(() => _dictionary[GetEntity()]);

    [Fact]
    public void Indexer_Get_YieldsProperValueForKey()
    {
        var entity = GetEntity();
        Component value = new(42);
        _dictionary.TryAdd(entity, value);
        Assert.Equal(value, _dictionary[entity]);
    }

    [Fact]
    public void Indexer_Set_CreatesEntryIfKeyNotFound()
    {
        var entity = GetEntity();
        Component value = new(42);
        _dictionary[entity] = value;
        Assert.True(_dictionary.Contains(entity));
        Assert.Equal(value, _dictionary[entity]);
    }

    [Fact]
    public void Indexer_Set_UpdatesEntryIfExists()
    {
        var entity = GetEntity();
        _dictionary.TryAdd(entity, new Component(42));
        var newValue = new Component(63);
        _dictionary[entity] = newValue;
        Assert.Equal(newValue, _dictionary[entity]);
    }

    [Fact]
    public void TryAdd_IncreasesCount()
    {
        _dictionary.TryAdd(GetEntity(), new Component(42));
        Assert.Equal(1, _dictionary.Count);
    }

    [Fact]
    public void TryAdd_EntriesWithIdenticalKeysDisallowed()
    {
        var entity = GetEntity();
        var value1 = new Component(42);
        var value2 = new Component(63);
        Assert.True(_dictionary.TryAdd(entity, value1));
        Assert.False(_dictionary.TryAdd(entity, value2));
        Assert.Equal(value1, _dictionary[entity]);
    }

    [Fact]
    public void TryAdd_TrueWhenNotFound() =>
        Assert.True(_dictionary.TryAdd(GetEntity(), new Component(42)));

    [Fact]
    public void TryAdd_FalseWhenExists()
    {
        var entity = GetEntity();
        var value = new Component(42);
        _dictionary.TryAdd(entity, value);
        Assert.False(_dictionary.TryAdd(entity, value));
    }

    [Fact]
    public void Contains_TrueWhenExists()
    {
        var entity = GetEntity();
        _dictionary.TryAdd(entity, new Component(42));
        Assert.True(_dictionary.Contains(entity));
    }

    [Fact]
    public void Contains_FalseWhenNotFound() =>
        Assert.False(_dictionary.Contains(GetEntity()));

    [Fact]
    public void Contains_FalseAfterRemoval()
    {
        var entity = GetEntity();
        _dictionary.TryAdd(entity, new Component(42));
        _dictionary.Remove(entity);
        Assert.False(_dictionary.Contains(entity));
    }

    [Fact]
    public void Remove_DecreasesCount()
    {
        var entity = GetEntity();
        _dictionary.TryAdd(entity, new Component(42));
        Assert.Equal(1, _dictionary.Count);
        _dictionary.Remove(entity);
        Assert.Equal(0, _dictionary.Count);
    }

    [Fact]
    public void Remove_TrueIfExisted()
    {
        var entity = GetEntity();
        _dictionary.TryAdd(entity, new Component());
        Assert.True(_dictionary.Remove(entity));
    }

    [Fact]
    public void Remove_FalseIfDidNotFind() =>
        Assert.False(_dictionary.Remove(GetEntity()));

    [Fact]
    public void Remove_CannotGetAfter()
    {
        var entity = GetEntity();
        _dictionary.TryAdd(entity, new Component(42));
        _dictionary.Remove(entity);
        Assert.Throws<ArgumentOutOfRangeException>(() => _ = _dictionary[entity]);
    }

    [Fact]
    public void Clear_SameAsEmptyInstance()
    {
        foreach (int i in Enumerable.Range(0, 5))
        {
            _dictionary.TryAdd(GetEntity(), new Component(i));
        }
        _dictionary.Clear();
        var emptyDictionary = new SparseDictionary<Entity, Component>(IndexExtractor.Entity);
        Assert.Equal(emptyDictionary.Count, _dictionary.Count);
        Assert.True(
            _dictionary.Keys.OrderBy(entity => entity.Id).SequenceEqual(emptyDictionary.Keys.OrderBy(entity => entity)));
        Assert.True(
            _dictionary.Values.OrderBy(component => component.Value)
                .SequenceEqual(emptyDictionary.Values.OrderBy(component => component.Value)));
    }

    [Fact]
    public void TryGet_TrueIfExists()
    {
        var entity = GetEntity();
        _dictionary.TryAdd(entity, new Component(42));
        Assert.True(_dictionary.TryGet(entity, out _));
    }

    [Fact]
    public void TryGet_FalseIfNotFound() =>
        Assert.False(_dictionary.TryGet(GetEntity(), out _));

    [Fact]
    public void TryGet_ResultEqualsJustAdded()
    {
        var entity = GetEntity();
        var component = new Component(42);
        _dictionary.TryAdd(entity, component);
        _dictionary.TryGet(entity, out var result);
        Assert.Equal(component, result);
    }

    [Fact]
    public void TrySet_TrueIfExists()
    {
        var entity = GetEntity();
        _dictionary.TryAdd(entity, new Component(42));
        Assert.True(_dictionary.TrySet(entity, new Component(63)));
    }

    [Fact]
    public void TrySet_FalseIfNotFound() =>
        Assert.False(_dictionary.TrySet(GetEntity(), new Component(42)));

    [Fact]
    public void TrySet_ValueChangesIfTrue()
    {
        var entity = GetEntity();
        _dictionary.TryAdd(entity, new Component(42));
        var newComponent = new Component(63);
        Assert.True(_dictionary.TrySet(entity, newComponent));
        Assert.Equal(newComponent, _dictionary[entity]);
    }

    [Fact]
    public void GetByRef_ThrowsIfNotFound() =>
        Assert.Throws<ArgumentOutOfRangeException>(() => _dictionary.GetByRef(GetEntity()));

    [Fact]
    public void GetByRef_MutableStructChanges()
    {
        var entity = GetEntity();
        _dictionary.TryAdd(entity, new Component(42));
        ref var component = ref _dictionary.GetByRef(entity);
        component.Value = 63;
        Assert.Equal(63, _dictionary[entity].Value);
    }

    private Entity GetEntity() =>
        _entitySpace.Create();

    private record struct Component(float Value);
}
