using System;
using System.Collections.Generic;

using Alitz.Ecs.Collections;

namespace Alitz.Ecs;
public partial class Environment
{
    public class ComponentDictionary<TComponent> : Collections.IDictionary<Entity, TComponent>, IDictionary<Entity>
        where TComponent : struct
    {
        public ComponentDictionary(EntitySpace entitySpace)
        {
            _entitySpace = entitySpace;
        }

        private readonly SparseDictionary<Entity, TComponent> _dictionary = new(IndexExtractor.Entity);
        private readonly EntitySpace _entitySpace;

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

        Type IDictionary<Entity>.ValueType =>
            ((IDictionary<Entity>)_dictionary).ValueType;

        IEnumerable<Entity> IDictionary<Entity>.Keys =>
            ((IDictionary<Entity>)_dictionary).Keys;

        IEnumerable<object> IDictionary<Entity>.Values =>
            ((IDictionary<Entity>)_dictionary).Values;

        object IDictionary<Entity>.this[Entity key]
        {
            get => ((IDictionary<Entity>)_dictionary)[ValidateEntity(key)];
            set => ((IDictionary<Entity>)_dictionary)[ValidateEntity(key)] = value;
        }

        bool IDictionary<Entity>.TryAdd(Entity key, object value) =>
            ((IDictionary<Entity>)_dictionary).TryAdd(ValidateEntity(key), value);

        bool IDictionary<Entity>.Contains(Entity key) =>
            ((IDictionary<Entity>)_dictionary).Contains(ValidateEntity(key));

        bool IDictionary<Entity>.Remove(Entity key) =>
            ((IDictionary<Entity>)_dictionary).Remove(ValidateEntity(key));

        bool IDictionary<Entity>.TryGet(Entity key, out object value) =>
            ((IDictionary<Entity>)_dictionary).TryGet(ValidateEntity(key), out value);

        bool IDictionary<Entity>.TrySet(Entity key, object value) =>
            ((IDictionary<Entity>)_dictionary).TrySet(ValidateEntity(key), value);

        private Entity ValidateEntity(Entity entity) =>
            _entitySpace.Exists(entity) ? entity : throw new EntityDoesNotExistException(entity);
    }
}
