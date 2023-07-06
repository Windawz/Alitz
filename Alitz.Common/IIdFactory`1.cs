namespace Alitz;
public interface IIdFactory<TId> where TId : struct, IId<TId>
{
    int MinIndex { get; }
    int MinVersion { get; }
    int MaxIndex { get; }
    int MaxVersion { get; }

    TId Create(int index, int version);
}
