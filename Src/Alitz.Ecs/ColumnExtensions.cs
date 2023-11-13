using System;

using Alitz.Common;

namespace Alitz.Ecs;
public static class ColumnExtensions
{
    public static void AddDefault<TComponent>(this Column<TComponent> column, Id entity) where TComponent : struct =>
        column.Add(entity, default!);

    public static void Add<TComponent>(this Column<TComponent> column, Id entity, TComponent component)
        where TComponent : struct
    {
        if (!column.TryAdd(entity, component))
        {
            throw new ArgumentOutOfRangeException(nameof(entity));
        }
    }

    public static bool TryAddDefault<TComponent>(this Column<TComponent> column, Id entity) where TComponent : struct =>
        column.TryAdd(entity, default!);

    public static TComponent Get<TComponent>(this Column<TComponent> column, Id entity) where TComponent : struct
    {
        if (column.TryGet(entity, out var value))
        {
            return value;
        }
        throw new ArgumentOutOfRangeException(nameof(entity));
    }

    public static void Set<TComponent>(this Column<TComponent> column, Id entity, TComponent component)
        where TComponent : struct
    {
        if (!column.TrySet(entity, component))
        {
            throw new ArgumentOutOfRangeException(nameof(entity));
        }
    }
}
