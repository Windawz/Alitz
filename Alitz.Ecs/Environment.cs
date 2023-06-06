using System;
using System.Collections.Generic;

using Alitz.Ecs.Collections;

namespace Alitz.Ecs;
public class Environment
{
    private readonly Dictionary<Type, IDictionary> _dictionaries = new();
    private readonly EntitySpace _entitySpace = new();

    public IEnumerable<Entity> Entities =>
        _entitySpace.Entities;

    public Entity CreateEntity() =>
        _entitySpace.Create();

    public bool EntityExists(Entity entity) =>
        _entitySpace.Exists(entity);

    public void DestroyEntity(Entity entity) =>
        _entitySpace.Destroy(entity);

    public Collections.IDictionary<Entity, TComponent> GetComponent<TComponent>() where TComponent : struct
    {
        var componentType = typeof(TComponent);
        if (!_dictionaries.ContainsKey(componentType))
        {
            var dictionary = new EntityAwareComponentDictionary<TComponent>(_entitySpace);
            _dictionaries.Add(componentType, dictionary);
        }
        return (Collections.IDictionary<Entity, TComponent>)_dictionaries[componentType];
    }
}
