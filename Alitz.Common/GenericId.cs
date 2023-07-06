using System;
using System.Numerics;

namespace Alitz;
public readonly struct GenericId : IId<GenericId>
{
    private const ulong IndexMask = 0x_7FFF_FFFF;
    private const ulong VersionMask = 0x_7FFF_FFFF_0000_0000;

    public GenericId() : this(MinIndex) { }

    public GenericId(int index) : this(index, MinVersion) { }

    public GenericId(int index, int version)
    {
        if (index < MinIndex || index > MaxIndex)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }
        if (version < MinVersion || version > MaxVersion)
        {
            throw new ArgumentOutOfRangeException(nameof(version));
        }
        ulong data = 0;
        SetIndex(ref data, InterpretAsUInt64(index));
        SetVersion(ref data, InterpretAsUInt64(version));
        _data = data;
    }

    public static readonly int MinIndex = 0;
    public static readonly int MinVersion = 0;
    public static readonly int MaxIndex = InterpretAsInt32(IndexMask);
    public static readonly int MaxVersion = InterpretAsInt32(VersionMask >> BitOperations.TrailingZeroCount(VersionMask));
    private readonly ulong _data;

    public int Index =>
        InterpretAsInt32(GetIndex(_data));

    public int Version =>
        InterpretAsInt32(GetVersion(_data));

    public bool Equals(GenericId other) =>
        Index == other.Index && Version == other.Version;

    public override bool Equals(object? obj) =>
        // Since this will be used as a "base" type
        // through composition, there's no way to check
        // equality from here. Otherwise it may return true
        // on two unrelated id types.
        // It's a sin, but for now I don't see a better option.
        throw new NotSupportedException();

    public override int GetHashCode() =>
        _data.GetHashCode();

    private static ulong GetIndex(ulong data) =>
        data & IndexMask;

    private static ulong GetVersion(ulong data) =>
        (data & VersionMask) >> BitOperations.TrailingZeroCount(VersionMask);

    private static void SetIndex(ref ulong data, ulong id) =>
        data |= id & IndexMask;

    private static void SetVersion(ref ulong data, ulong version) =>
        data |= version << BitOperations.TrailingZeroCount(VersionMask) & VersionMask;

    private static unsafe int InterpretAsInt32(ulong value)
    {
        var span = new ReadOnlySpan<byte>(&value, sizeof(ulong));
        return BitConverter.ToInt32(span);
    }

    private static unsafe ulong InterpretAsUInt64(int value)
    {
        // BitConverter.ToUInt64() requires at least 8 bytes in the span.
        long longValue = value;
        var span = new ReadOnlySpan<byte>(&longValue, sizeof(long));
        return BitConverter.ToUInt64(span);
    }

    public class Factory : IIdFactory<GenericId>
    {
        public int MinIndex { get; } = GenericId.MinIndex;
        public int MinVersion { get; } = GenericId.MinVersion;
        public int MaxIndex { get; } = GenericId.MaxIndex;
        public int MaxVersion { get; } = GenericId.MaxVersion;

        public GenericId Create(int index, int version) =>
            new(index, version);
    }
}
