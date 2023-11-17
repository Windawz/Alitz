using System;
using System.Collections.Generic;
using System.Linq;

using Alitz.Common.Collections;

namespace Alitz.EntityComponentSystem;
public class Ecs
{
    public Ecs(IReadOnlyCollection<ISystem> systems)
    {
        _systemContext = new SystemContext(new IdPool(), new Dictionary<Type, IColumn>());
        _systems = systems.ToArray();
    }

    private readonly ISystemContext _systemContext;
    private readonly IReadOnlyCollection<ISystem> _systems;

    public void Update(long deltaMs)
    {
        foreach (var system in _systems)
        {
            system.Update(_systemContext, deltaMs);
        }
    }
}
