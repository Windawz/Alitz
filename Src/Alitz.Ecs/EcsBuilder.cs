using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using Alitz.Ecs.Systems;

namespace Alitz.Ecs;
public class EcsBuilder
{
    internal EcsBuilder() { }

    private readonly List<SystemInstantiationInfo> _instantiationInfos = new();

    public EcsBuilder AddSystems(IEnumerable<SystemType> systemTypes)
    {
        if (systemTypes.TryGetNonEnumeratedCount(out int count))
        {
            _instantiationInfos.EnsureCapacity(_instantiationInfos.Count + count);
        }
        foreach (var systemType in systemTypes)
        {
            AddSystem(systemType);
        }
        return this;
    }

    public EcsBuilder AddSystems(IEnumerable<(SystemType, Func<ISystem>)> systemTypesAndFactories)
    {
        if (systemTypesAndFactories.TryGetNonEnumeratedCount(out int count))
        {
            _instantiationInfos.EnsureCapacity(_instantiationInfos.Count + count);
        }
        foreach (var (systemType, factory) in systemTypesAndFactories)
        {
            AddSystem(systemType, factory);
        }
        return this;
    }

    public EcsBuilder AddSystem(SystemType systemType, Func<ISystem>? factory = null)
    {
        _instantiationInfos.Add(new SystemInstantiationInfo(systemType, factory));
        return this;
    }

    public EcsBuilder AddSystem<TSystem>() where TSystem : class, ISystem, new()
    {
        _instantiationInfos.Add(new SystemInstantiationInfo(typeof(TSystem), null));
        return this;
    }

    public EcsBuilder AddSystem<TSystem>(Func<TSystem>? factory) where TSystem : class, ISystem
    {
        _instantiationInfos.Add(new SystemInstantiationInfo(typeof(TSystem), factory));
        return this;
    }

    public EntityComponentSystem Build()
    {
        var graph = new DependencyGraph(_instantiationInfos.Select(info => info.SystemType));
        var schedule = new SystemSchedule(graph);
        var systems = schedule
            .Join(
                inner: _instantiationInfos,
                outerKeySelector: systemType => systemType.Type,
                innerKeySelector: info => info.SystemType.Type,
                resultSelector: (systemType, info) => (systemType, systemFactory: info.SystemFactory)
            ).Select(tuple => tuple.systemFactory())
            .ToArray();
        return new EntityComponentSystem(systems);
    }
}
