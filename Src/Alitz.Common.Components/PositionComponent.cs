namespace Alitz.Common.Components;
public readonly struct PositionComponent
{
    public PositionComponent(long x, long y)
    {
        X = x;
        Y = y;
    }

    public long X { get; }
    public long Y { get; }

    public static implicit operator PositionComponent((long x, long y) positionTuple) =>
        new(positionTuple.x, positionTuple.y);
}
