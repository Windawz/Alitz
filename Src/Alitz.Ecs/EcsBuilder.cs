using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using Alitz.Ecs.Systems;
using Alitz.Ecs.Systems.Scheduling;

namespace Alitz.Ecs;
public class EcsBuilder
{
    internal EcsBuilder() { }

    private readonly List<SystemFactory> _factories = new();

    public EcsBuilder AddSystems(IEnumerable<Type> systemTypes)
    {
        PrepareFactoryListForInsertionOfEnumerable(systemTypes);
        foreach (var systemType in systemTypes)
        {
            AddSystem(systemType);
        }
        return this;
    }

    public EcsBuilder AddSystems(IEnumerable<(Type, Func<ISystem>)> systemTypesAndFactories)
    {
        PrepareFactoryListForInsertionOfEnumerable(systemTypesAndFactories);
        foreach (var (systemType, factory) in systemTypesAndFactories)
        {
            AddSystem(systemType, factory);
        }
        return this;
    }

    public EcsBuilder AddSystem(Type systemType, Func<ISystem>? factory = null)
    {
        bool prevalidateFactoryType = factory is not null;
        factory ??= MakeFactoryFromParameterlessConstructorOrThrowIfNone(systemType);
        _factories.Add(new SystemFactory(systemType, factory, prevalidateFactoryType));
        return this;
    }

    public EcsBuilder AddSystem<TSystem>() where TSystem : class, ISystem, new()
    {
        _factories.Add(new SystemFactory(typeof(TSystem), () => new TSystem(), false));
        return this;
    }

    public EcsBuilder AddSystem<TSystem>(Func<TSystem>? factory) where TSystem : class, ISystem
    {
        bool prevalidateFactoryType = factory is not null;
        factory ??= (Func<TSystem>)MakeFactoryFromParameterlessConstructorOrThrowIfNone(typeof(TSystem));
        _factories.Add(new SystemFactory(typeof(TSystem), factory, prevalidateFactoryType));
        return this;
    }

    public EntityComponentSystem Build()
    {
        var schedule = new SystemSchedule(_factories);
        return new EntityComponentSystem(schedule);
    }

    private void PrepareFactoryListForInsertionOfEnumerable<T>(IEnumerable<T> enumerable)
    {
        if (enumerable.TryGetNonEnumeratedCount(out int count))
        {
            _factories.EnsureCapacity(_factories.Count + count);
        }
    }

    private static Func<ISystem> MakeFactoryFromParameterlessConstructorOrThrowIfNone(Type systemType)
    {
        var parameterlessConstructor = systemType.GetConstructor(
            BindingFlags.Public | BindingFlags.Instance,
            Array.Empty<Type>());

        if (parameterlessConstructor is null)
        {
            throw new ArgumentException(
                $"System type {systemType} "
                + "has no public parameterless constructor and no custom factory was provided");
        }

        return Expression.Lambda<Func<ISystem>>(Expression.New(parameterlessConstructor)).Compile();
    }
}
