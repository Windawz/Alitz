namespace Alitz.Querying;
public static partial class EnvironmentExtensions
{
    public delegate void ForEachAction<TComponent1>(Entity entity, ref TComponent1 component1) where TComponent1 : struct;

    public delegate void ForEachAction<TComponent1, TComponent2>(
        Entity entity,
        ref TComponent1 component1,
        ref TComponent2 component2
    ) where TComponent1 : struct where TComponent2 : struct;

    public delegate void ForEachAction<TComponent1, TComponent2, TComponent3>(
        Entity entity,
        ref TComponent1 component1,
        ref TComponent2 component2,
        ref TComponent3 component3
    ) where TComponent1 : struct where TComponent2 : struct where TComponent3 : struct;

    public delegate void ForEachAction<TComponent1, TComponent2, TComponent3, TComponent4>(
        Entity entity,
        ref TComponent1 component1,
        ref TComponent2 component2,
        ref TComponent3 component3,
        ref TComponent4 component4
    ) where TComponent1 : struct where TComponent2 : struct where TComponent3 : struct where TComponent4 : struct;

    public static void ForEach<TComponent1>(this Environment environment, ForEachAction<TComponent1> action)
        where TComponent1 : struct
    {
        var components1 = environment.Components<TComponent1>();
        foreach (var entity in components1.Keys)
        {
            action(entity, ref components1.GetByRef(entity));
        }
    }

    public static void ForEach<TComponent1, TComponent2>(
        this Environment environment,
        ForEachAction<TComponent1, TComponent2> action
    ) where TComponent1 : struct where TComponent2 : struct
    {
        var components1 = environment.Components<TComponent1>();
        var components2 = environment.Components<TComponent2>();
        using var enumerator = new IntersectionEnumerator(components1, components2);
        while (enumerator.MoveNext())
        {
            var entity = enumerator.Current;
            action(enumerator.Current, ref components1.GetByRef(entity), ref components2.GetByRef(entity));
        }
    }

    public static void ForEach<TComponent1, TComponent2, TComponent3>(
        this Environment environment,
        ForEachAction<TComponent1, TComponent2, TComponent3> action
    ) where TComponent1 : struct where TComponent2 : struct where TComponent3 : struct
    {
        var components1 = environment.Components<TComponent1>();
        var components2 = environment.Components<TComponent2>();
        var components3 = environment.Components<TComponent3>();
        using var enumerator = new IntersectionEnumerator(components1, components2, components3);
        while (enumerator.MoveNext())
        {
            var entity = enumerator.Current;
            action(
                enumerator.Current,
                ref components1.GetByRef(entity),
                ref components2.GetByRef(entity),
                ref components3.GetByRef(entity));
        }
    }

    public static void ForEach<TComponent1, TComponent2, TComponent3, TComponent4>(
        this Environment environment,
        ForEachAction<TComponent1, TComponent2, TComponent3, TComponent4> action
    ) where TComponent1 : struct where TComponent2 : struct where TComponent3 : struct where TComponent4 : struct
    {
        var components1 = environment.Components<TComponent1>();
        var components2 = environment.Components<TComponent2>();
        var components3 = environment.Components<TComponent3>();
        var components4 = environment.Components<TComponent4>();
        using var enumerator = new IntersectionEnumerator(components1, components2, components3, components4);
        while (enumerator.MoveNext())
        {
            var entity = enumerator.Current;
            action(
                enumerator.Current,
                ref components1.GetByRef(entity),
                ref components2.GetByRef(entity),
                ref components3.GetByRef(entity),
                ref components4.GetByRef(entity));
        }
    }
}
