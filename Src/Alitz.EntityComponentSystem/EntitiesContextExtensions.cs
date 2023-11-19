namespace Alitz.EntityComponentSystem;
public static class EntitiesContextExtensions
{
    public delegate void ForEachAction<TComponent1>(
        IEntityContext entityContext,
        ref TComponent1 component1
    ) 
        where TComponent1 : struct;

    public delegate void ForEachAction<TComponent1, TComponent2>(
        IEntityContext entityContext,
        ref TComponent1 component1,
        ref TComponent2 component2
    ) 
        where TComponent1 : struct
        where TComponent2 : struct;

    public delegate void ForEachAction<TComponent1, TComponent2, TComponent3>(
        IEntityContext entityContext,
        ref TComponent1 component1,
        ref TComponent2 component2,
        ref TComponent3 component3
    ) 
        where TComponent1 : struct
        where TComponent2 : struct
        where TComponent3 : struct;

    public delegate void ForEachAction<TComponent1, TComponent2, TComponent3, TComponent4>(
        IEntityContext entityContext,
        ref TComponent1 component1,
        ref TComponent2 component2,
        ref TComponent3 component3,
        ref TComponent4 component4
    ) 
        where TComponent1 : struct
        where TComponent2 : struct
        where TComponent3 : struct
        where TComponent4 : struct;

    public static void ForEach<TComponent1>(this IEntitiesContext context, ForEachAction<TComponent1> action)
        where TComponent1 : struct
    {
        context.RawForEach(
            enumeratorFactory: (entityPool, table) =>
            {
                return table.Column<TComponent1>().Entities.GetEnumerator();
            },
            action: (entityContext, table) =>
            {
                action(entityContext, ref table.Column<TComponent1>().GetByRef(entityContext.Entity));
            }
        );
    }

    public static void ForEach<TComponent1, TComponent2>(
        this IEntitiesContext context,
        ForEachAction<TComponent1, TComponent2> action
    )
        where TComponent1 : struct
        where TComponent2 : struct
    {
        context.RawForEach(
            enumeratorFactory: (entityPool, table) =>
            {
                return new ColumnIntersectionEnumerator(
                    table.Column<TComponent1>(),
                    table.Column<TComponent2>()
                );
            },
            action: (entityContext, table) =>
            {
                action(
                    entityContext,
                    ref table.Column<TComponent1>().GetByRef(entityContext.Entity),
                    ref table.Column<TComponent2>().GetByRef(entityContext.Entity)
                );
            }
        );
    }

    public static void ForEach<TComponent1, TComponent2, TComponent3>(
        this IEntitiesContext context,
        ForEachAction<TComponent1, TComponent2, TComponent3> action
    )
        where TComponent1 : struct
        where TComponent2 : struct
        where TComponent3 : struct
    {
        context.RawForEach(
            enumeratorFactory: (entityPool, table) =>
            {
                return new ColumnIntersectionEnumerator(
                    table.Column<TComponent1>(),
                    table.Column<TComponent2>(),
                    table.Column<TComponent3>()
                );
            },
            action: (entityContext, table) =>
            {
                action(
                    entityContext,
                    ref table.Column<TComponent1>().GetByRef(entityContext.Entity),
                    ref table.Column<TComponent2>().GetByRef(entityContext.Entity),
                    ref table.Column<TComponent3>().GetByRef(entityContext.Entity)
                );
            }
        );
    }

    public static void ForEach<TComponent1, TComponent2, TComponent3, TComponent4>(
        this IEntitiesContext context,
        ForEachAction<TComponent1, TComponent2, TComponent3, TComponent4> action
    )
        where TComponent1 : struct
        where TComponent2 : struct
        where TComponent3 : struct
        where TComponent4 : struct
    {
        context.RawForEach(
            enumeratorFactory: (entityPool, table) =>
            {
                return new ColumnIntersectionEnumerator(
                    table.Column<TComponent1>(),
                    table.Column<TComponent2>(),
                    table.Column<TComponent3>(),
                    table.Column<TComponent4>()
                );
            },
            action: (entityContext, table) =>
            {
                action(
                    entityContext,
                    ref table.Column<TComponent1>().GetByRef(entityContext.Entity),
                    ref table.Column<TComponent2>().GetByRef(entityContext.Entity),
                    ref table.Column<TComponent3>().GetByRef(entityContext.Entity),
                    ref table.Column<TComponent4>().GetByRef(entityContext.Entity)
                );
            }
        );
    }
}
