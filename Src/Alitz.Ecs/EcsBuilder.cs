using System;
using System.Collections.Generic;

using Alitz.Systems;

namespace Alitz;
public class EcsBuilder
{
    internal EcsBuilder() { }

    private readonly List<SystemFactory> _factories = new();

    public EcsBuilder AddSystem<TSystem>() where TSystem : class, ISystem, new()
    {
        _factories.Add(SystemFactory.Create(() => new TSystem()));
        return this;
    }

    public EcsBuilder AddSystem<TSystem>(Func<TSystem> factory) where TSystem : class, ISystem
    {
        _factories.Add(SystemFactory.Create(factory));
        return this;
    }

    public EntityComponentSystem Build()
    {
        var schedule = new SystemSchedule(_factories);
        return new EntityComponentSystem(schedule);
    }
}
