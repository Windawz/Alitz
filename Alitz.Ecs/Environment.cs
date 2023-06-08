using System;
using System.Collections.Generic;

using Alitz.Ecs.Collections;

namespace Alitz.Ecs;
public partial class Environment
{
    private readonly Dictionary<Type, IDictionary<Entity>> _dictionaries = new();
    private readonly EntitySpace _entitySpace = new();

    public IEnumerable<Entity> Entities =>
        _entitySpace.Entities;

    public Entity Create() =>
        _entitySpace.Create();

    public bool Exists(Entity entity) =>
        _entitySpace.Exists(entity);

    public void Destroy(Entity entity) =>
        _entitySpace.Destroy(entity);

    public ComponentDictionary<TComponent> Components<TComponent>() where TComponent : struct
    {
        var componentType = typeof(TComponent);
        if (!_dictionaries.ContainsKey(componentType))
        {
            var dictionary = new ComponentDictionary<TComponent>(_entitySpace);
            _dictionaries.Add(componentType, dictionary);
        }
        return (ComponentDictionary<TComponent>)_dictionaries[componentType];
    }
}
