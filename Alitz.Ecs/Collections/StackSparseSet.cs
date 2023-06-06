namespace Alitz.Ecs.Collections;
internal class StackSparseSet<T> : SparseSet<T>
{
    public StackSparseSet(IndexExtractor<T> indexExtractor) : base(indexExtractor) { }

    public bool TryPeek(out T? value)
    {
        if (Count > 0)
        {
            value = Dense[^1];
            return true;
        }
        value = default;
        return false;
    }

    public bool TryPop(out T? value)
    {
        if (Count == 0)
        {
            value = default;
            return false;
        }
        value = Dense[^1];
        Remove(value);
        return true;
    }
}
