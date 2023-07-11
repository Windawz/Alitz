using System;

namespace Alitz.UnitTests;
public readonly struct MockId : IId<MockId>
{
    public MockId(int index, int version)
    {
        if (index < MinIndex || index > MaxIndex)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }
        if (version < MinVersion || version > MaxVersion)
        {
            throw new ArgumentOutOfRangeException(nameof(version));
        }
        Index = index;
        Version = version;
    }

    public static readonly int MinIndex = 0;
    public static readonly int MinVersion = 0;
    public static readonly int MaxIndex = int.MaxValue;
    public static readonly int MaxVersion = int.MaxValue;

    public int Index { get; }

    public int Version { get; }

    public bool Equals(MockId other) =>
        Index == other.Index && Version == other.Version;
}
