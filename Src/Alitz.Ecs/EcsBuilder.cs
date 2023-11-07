using System;
using System.Collections.Generic;
using System.Linq;

using Alitz.Ecs.Systems;

namespace Alitz.Ecs;
public class EcsBuilder
{
    internal EcsBuilder() { }

    private readonly List<SystemInstantiationInfo> _instantiationInfos = new();

    public EcsBuilder AddSystems(IEnumerable<Type> systemTypes)
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

    public EcsBuilder AddSystems(IEnumerable<(Type, Func<ISystem>)> systemTypesAndFactories)
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

    public EcsBuilder AddSystem(Type systemType, Func<ISystem>? factory = null)
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
        var schedule = new SystemSchedule(
            _instantiationInfos.Select(info => info.SystemType)
        );

        return new EntityComponentSystem(
            schedule.Join(
                _instantiationInfos,
                systemType => systemType,
                info => info.SystemType,
                (_, info) => info.SystemFactory()
            )
            .ToArray()
        );
    }
}
