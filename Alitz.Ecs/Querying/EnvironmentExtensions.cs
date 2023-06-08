namespace Alitz.Ecs.Querying;
public static partial class EnvironmentExtensions
{
    public static bool TryGetComponent<TComponent>(this Environment environment, Entity entity, out TComponent component)
        where TComponent : struct
    {
        component = default!;
        if (!environment.Exists(entity))
        {
            return false;
        }
        var components = environment.Components<TComponent>();
        return components.TryGet(entity, out component);
    }

    public static void SetComponent<TComponent>(this Environment environment, Entity entity, TComponent value)
        where TComponent : struct =>
        environment.Components<TComponent>()[entity] = value;

    public static ref TComponent Component<TComponent>(this Environment environment, Entity entity)
        where TComponent : struct =>
        ref environment.Components<TComponent>().GetByRef(entity);
}
