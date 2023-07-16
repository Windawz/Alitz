using System;

namespace Alitz.Collections;
public static class ColumnExtensions
{
    public static void AddDefault<TComponent>(this IColumn<TComponent> column, Id entity) where TComponent : struct =>
        column.Add(entity, default!);

    public static void Add<TComponent>(this IColumn<TComponent> column, Id entity, TComponent component)
        where TComponent : struct
    {
        if (!column.TryAdd(entity, component))
        {
            throw new ArgumentOutOfRangeException(nameof(entity));
        }
    }

    public static bool TryAddDefault<TComponent>(this IColumn<TComponent> column, Id entity) where TComponent : struct =>
        column.TryAdd(entity, default!);

    public static TComponent Get<TComponent>(this IColumn<TComponent> column, Id entity) where TComponent : struct
    {
        if (column.TryGet(entity, out var value))
        {
            return value;
        }
        throw new ArgumentOutOfRangeException(nameof(entity));
    }

    public static void Set<TComponent>(this IColumn<TComponent> column, Id entity, TComponent component)
        where TComponent : struct
    {
        if (!column.TrySet(entity, component))
        {
            throw new ArgumentOutOfRangeException(nameof(entity));
        }
    }
}
