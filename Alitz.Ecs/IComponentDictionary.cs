using System;
using System.Collections.Generic;

namespace Alitz.Ecs; 
public interface IComponentDictionary {
    Type ComponentType { get; }
    int Count { get; }
    IReadOnlyList<Entity> Entities { get; }
    
    int Add(Entity entity);
    bool Contains(Entity entity);
    bool Remove(Entity entity);
}
