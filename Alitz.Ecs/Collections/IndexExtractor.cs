namespace Alitz.Collections;
public static class IndexExtractor
{
    public static IndexExtractor<Entity> Entity { get; } = new(static value => value.Id);
}
