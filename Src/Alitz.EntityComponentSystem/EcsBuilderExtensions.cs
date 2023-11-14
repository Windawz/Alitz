using System;
using System.Collections.Generic;



namespace Alitz.EntityComponentSystem;
public static class EcsBuilderExtensions
{
    public static EcsBuilder AddSystem<TSystem>(this EcsBuilder builder) where TSystem : class, ISystem, new()
    {
        builder.AddSystem(typeof(TSystem));
        return builder;
    }

    public static EcsBuilder AddSystem<TSystem>(this EcsBuilder builder, Func<TSystem>? factory) where TSystem : class, ISystem
    {
        builder.AddSystem(typeof(TSystem), factory);
        return builder;
    }
    
    public static EcsBuilder AddSystems(this EcsBuilder builder, IEnumerable<Type> systemTypes)
    {
        foreach (var systemType in systemTypes)
        {
            builder.AddSystem(systemType);
        }
        return builder;
    }
}