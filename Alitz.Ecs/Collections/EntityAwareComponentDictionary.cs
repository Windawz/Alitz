using System;
using System.Collections.Generic;

namespace Alitz.Ecs.Collections;
internal class EntityAwareComponentDictionary<TComponent> : IDictionary<Entity, TComponent>, IDictionary
    where TComponent : struct
{
    public EntityAwareComponentDictionary(EntitySpace entitySpace)
    {
        _entitySpace = entitySpace;
    }

    private readonly SparseDictionary<Entity, TComponent> _dictionary = new(IndexExtractor.Entity);
    private readonly EntitySpace _entitySpace;

    Type IDictionary.KeyType =>
        ((IDictionary)_dictionary).KeyType;

    Type IDictionary.ValueType =>
        ((IDictionary)_dictionary).ValueType;

    IEnumerable<object> IDictionary.Keys =>
        ((IDictionary)_dictionary).Keys;

    IEnumerable<object> IDictionary.Values =>
        ((IDictionary)_dictionary).Values;

    object IDictionary.this[object key]
    {
        get => ((IDictionary)_dictionary)[ValidateEntity(key)];
        set => ((IDictionary)_dictionary)[ValidateEntity(key)] = value;
    }

    bool IDictionary.TryAdd(object key, object value) =>
        ((IDictionary)_dictionary).TryAdd(ValidateEntity(key), value);

    bool IDictionary.Contains(object key) =>
        ((IDictionary)_dictionary).Contains(ValidateEntity(key));

    bool IDictionary.Remove(object key) =>
        ((IDictionary)_dictionary).Remove(ValidateEntity(key));

    bool IDictionary.TryGet(object key, out object value) =>
        ((IDictionary)_dictionary).TryGet(ValidateEntity(key), out value);

    bool IDictionary.TrySet(object key, object value) =>
        ((IDictionary)_dictionary).TrySet(ValidateEntity(key), value);

    public int Count =>
        _dictionary.Count;

    public IEnumerable<Entity> Keys =>
        _dictionary.Keys;

    public IEnumerable<TComponent> Values =>
        _dictionary.Values;

    public TComponent this[Entity key]
    {
        get => _dictionary[key];
        set => _dictionary[ValidateEntity(key)] = value;
    }

    public bool TryAdd(Entity key, TComponent value) =>
        _dictionary.TryAdd(ValidateEntity(key), value);

    public bool Contains(Entity key) =>
        _dictionary.Contains(ValidateEntity(key));

    public bool Remove(Entity key) =>
        _dictionary.Remove(ValidateEntity(key));

    public void Clear() =>
        _dictionary.Clear();

    public bool TryGet(Entity key, out TComponent value) =>
        _dictionary.TryGet(ValidateEntity(key), out value);

    public bool TrySet(Entity key, TComponent value) =>
        _dictionary.TrySet(ValidateEntity(key), value);

    public ref TComponent GetByRef(Entity key) =>
        ref _dictionary.GetByRef(ValidateEntity(key));

    private object ValidateEntity(object entity) =>
        ValidateEntity((Entity)entity);

    private Entity ValidateEntity(Entity entity) =>
        _entitySpace.Exists(entity) ? entity : throw new EntityDoesNotExistException(entity);
}
