using Alitz.Common;

namespace Alitz.Ecs;
public static class SystemContextExtensions
{
    public delegate void DoAction<TComponent>(ref TComponent component) where TComponent : struct;

    public delegate void ForEachAction<TComponent1>(Id entity, ref TComponent1 component1) where TComponent1 : struct;

    public delegate void ForEachAction<TComponent1, TComponent2>(
        Id entity,
        ref TComponent1 component1,
        ref TComponent2 component2
    ) where TComponent1 : struct where TComponent2 : struct;

    public delegate void ForEachAction<TComponent1, TComponent2, TComponent3>(
        Id entity,
        ref TComponent1 component1,
        ref TComponent2 component2,
        ref TComponent3 component3
    ) where TComponent1 : struct where TComponent2 : struct where TComponent3 : struct;

    public delegate void ForEachAction<TComponent1, TComponent2, TComponent3, TComponent4>(
        Id entity,
        ref TComponent1 component1,
        ref TComponent2 component2,
        ref TComponent3 component3,
        ref TComponent4 component4
    ) where TComponent1 : struct where TComponent2 : struct where TComponent3 : struct where TComponent4 : struct;

    public static void ForEach<TComponent1>(this ISystemContext context, ForEachAction<TComponent1> action)
        where TComponent1 : struct
    {
        var components1 = context.Components<TComponent1>();
        foreach (var entity in components1.Entities)
        {
            action(entity, ref components1.GetByRef(entity));
        }
    }

    public static void ForEach<TComponent1, TComponent2>(
        this ISystemContext context,
        ForEachAction<TComponent1, TComponent2> action
    ) where TComponent1 : struct where TComponent2 : struct
    {
        var components1 = context.Components<TComponent1>();
        var components2 = context.Components<TComponent2>();
        using var enumerator = new ColumnIntersectionEnumerator(components1, components2);
        while (enumerator.MoveNext())
        {
            var entity = enumerator.Current;
            action(enumerator.Current, ref components1.GetByRef(entity), ref components2.GetByRef(entity));
        }
    }

    public static void ForEach<TComponent1, TComponent2, TComponent3>(
        this ISystemContext context,
        ForEachAction<TComponent1, TComponent2, TComponent3> action
    ) where TComponent1 : struct where TComponent2 : struct where TComponent3 : struct
    {
        var components1 = context.Components<TComponent1>();
        var components2 = context.Components<TComponent2>();
        var components3 = context.Components<TComponent3>();
        using var enumerator = new ColumnIntersectionEnumerator(components1, components2, components3);
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
        this ISystemContext context,
        ForEachAction<TComponent1, TComponent2, TComponent3, TComponent4> action
    ) where TComponent1 : struct where TComponent2 : struct where TComponent3 : struct where TComponent4 : struct
    {
        var components1 = context.Components<TComponent1>();
        var components2 = context.Components<TComponent2>();
        var components3 = context.Components<TComponent3>();
        var components4 = context.Components<TComponent4>();
        using var enumerator = new ColumnIntersectionEnumerator(components1, components2, components3, components4);
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

    public static void Do<TComponent>(this ISystemContext context, Id entity, DoAction<TComponent> action)
        where TComponent : struct =>
        action(ref context.Components<TComponent>().GetByRef(entity));

    public static void DoIfHas<TComponent>(this ISystemContext context, Id entity, DoAction<TComponent> action)
        where TComponent : struct
    {
        var column = context.Components<TComponent>();
        if (column.Contains(entity))
        {
            action(ref column.GetByRef(entity));
        }
    }
}
