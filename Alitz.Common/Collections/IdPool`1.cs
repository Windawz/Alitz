namespace Alitz.Collections;
public class IdPool<TId> : Pool<TId> where TId : struct, IId<TId>
{
    public IdPool(IIdFactory<TId> factory)
    {
        _factory = factory;
    }

    private readonly IIdFactory<TId> _factory;

    protected override TId Reuse(TId toBeReused) =>
        _factory.Create(toBeReused.Index, toBeReused.Version + 1);

    protected override TId Next(TId last) =>
        _factory.Create(last.Index + 1, _factory.MinVersion);

    protected override TId New() =>
        _factory.Create(_factory.MinIndex, _factory.MinVersion);
}
