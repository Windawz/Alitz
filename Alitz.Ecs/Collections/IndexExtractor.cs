namespace Alitz.Ecs.Collections;
public static class IndexExtractor
{
    public static IndexExtractor<Entity> Entity { get; } = static value => value.Id;
}
