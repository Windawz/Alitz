using System;
using System.Collections.Generic;

namespace Alitz.Ecs;
public class ComponentDictionary<TComponent> :
    SparseDictionary<Entity, EntityIndexProvider, TComponent>,
    IComponentDictionary
    where TComponent : struct {
    
    Type IComponentDictionary.ComponentType =>
        typeof(TComponent);
    
    IReadOnlyList<Entity> IComponentDictionary.Entities =>
        Keys;
}
