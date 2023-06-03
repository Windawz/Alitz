using System;
using System.Collections.Generic;

namespace Alitz.Ecs.Collections;
public class ComponentDictionary<TComponent> : IComponentDictionary<TComponent>, IComponentDictionary
    where TComponent : struct
{
    private readonly SparseDictionary<Entity, TComponent> _dictionary = new(IndexExtractors.EntityIndexExtractor);

    Type ISparseDictionary.KeyType =>
        DictionaryAsTypeless().KeyType;

    Type ISparseDictionary.ValueType =>
        DictionaryAsTypeless().ValueType;

    IEnumerable<object> ISparseDictionary.Keys =>
        DictionaryAsTypeless().Keys;

    IEnumerable<object> ISparseDictionary.Values =>
        DictionaryAsTypeless().Values;

    object ISparseDictionary.this[object key]
    {
        get => DictionaryAsTypeless()[key];
        set => DictionaryAsTypeless()[key] = value;
    }

    bool ISparseDictionary.TryAdd(object key, object value) =>
        DictionaryAsTypeless().TryAdd(key, value);

    bool ISparseDictionary.Contains(object key) =>
        DictionaryAsTypeless().Contains(key);

    bool ISparseDictionary.Remove(object key) =>
        DictionaryAsTypeless().Remove(key);

    bool ISparseDictionary.TryGet(object key, out object value) =>
        DictionaryAsTypeless().TryGet(key, out value);

    bool ISparseDictionary.TrySet(object key, object value) =>
        DictionaryAsTypeless().TrySet(key, value);

    public int Count =>
        _dictionary.Count;

    public IEnumerable<Entity> Keys =>
        _dictionary.Keys;

    public IEnumerable<TComponent> Values =>
        _dictionary.Values;

    public TComponent this[Entity key]
    {
        get => _dictionary[key];
        set => _dictionary[key] = value;
    }

    public bool TryAdd(Entity key, TComponent value) =>
        _dictionary.TryAdd(key, value);

    public bool Contains(Entity key) =>
        _dictionary.Contains(key);

    public bool Remove(Entity key) =>
        _dictionary.Remove(key);

    public void Clear() =>
        _dictionary.Clear();

    public bool TryGet(Entity key, out TComponent value) =>
        _dictionary.TryGet(key, out value);

    public bool TrySet(Entity key, TComponent value) =>
        _dictionary.TrySet(key, value);

    public ref TComponent GetByRef(Entity key) =>
        ref _dictionary.GetByRef(key);

    private ISparseDictionary DictionaryAsTypeless() =>
        _dictionary;
}
