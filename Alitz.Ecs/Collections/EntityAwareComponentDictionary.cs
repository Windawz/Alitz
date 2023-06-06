using System;
using System.Collections.Generic;

namespace Alitz.Ecs.Collections;
internal class EntityAwareComponentDictionary<TComponent> : ISparseDictionary<Entity, TComponent>, ISparseDictionary
    where TComponent : struct
{
    public EntityAwareComponentDictionary(EntitySpace entitySpace)
    {
        _entitySpace = entitySpace;
    }

    private readonly SparseDictionary<Entity, TComponent> _dictionary = new(IndexExtractors.EntityIndexExtractor);
    private readonly EntitySpace _entitySpace;

    Type ISparseDictionary.KeyType =>
        ((ISparseDictionary)_dictionary).KeyType;

    Type ISparseDictionary.ValueType =>
        ((ISparseDictionary)_dictionary).ValueType;

    IEnumerable<object> ISparseDictionary.Keys =>
        ((ISparseDictionary)_dictionary).Keys;

    IEnumerable<object> ISparseDictionary.Values =>
        ((ISparseDictionary)_dictionary).Values;

    object ISparseDictionary.this[object key]
    {
        get => ((ISparseDictionary)_dictionary)[ValidateEntity(key)];
        set => ((ISparseDictionary)_dictionary)[ValidateEntity(key)] = value;
    }

    bool ISparseDictionary.TryAdd(object key, object value) =>
        ((ISparseDictionary)_dictionary).TryAdd(ValidateEntity(key), value);

    bool ISparseDictionary.Contains(object key) =>
        ((ISparseDictionary)_dictionary).Contains(ValidateEntity(key));

    bool ISparseDictionary.Remove(object key) =>
        ((ISparseDictionary)_dictionary).Remove(ValidateEntity(key));

    bool ISparseDictionary.TryGet(object key, out object value) =>
        ((ISparseDictionary)_dictionary).TryGet(ValidateEntity(key), out value);

    bool ISparseDictionary.TrySet(object key, object value) =>
        ((ISparseDictionary)_dictionary).TrySet(ValidateEntity(key), value);

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
