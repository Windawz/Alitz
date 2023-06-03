namespace Alitz.Ecs.Collections;
public static class IndexExtractors
{
    public static IndexExtractor<Entity> EntityIndexExtractor { get; } = static value => value.Id;

    public static IndexExtractor<int> Int32IndexExtractor { get; } = static value => value;
}
