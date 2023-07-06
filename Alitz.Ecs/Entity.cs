namespace Alitz;
public readonly struct Entity : IId<Entity>
{
    public Entity()
    {
        _genericId = new GenericId();
    }

    public Entity(int index)
    {
        _genericId = new GenericId(index);
    }

    public Entity(int index, int version)
    {
        _genericId = new GenericId(index, version);
    }

    private readonly GenericId _genericId;

    public static int MinIndex =>
        GenericId.MinIndex;

    public static int MinVersion =>
        GenericId.MinVersion;

    public static int MaxIndex =>
        GenericId.MaxIndex;

    public static int MaxVersion =>
        GenericId.MaxVersion;

    public int Index =>
        _genericId.Index;

    public int Version =>
        _genericId.Version;

    public bool Equals(Entity other) =>
        _genericId.Equals(other._genericId);
}
