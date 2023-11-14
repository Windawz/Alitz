using System;
using System.Numerics;

namespace Alitz.Common;
public readonly struct Id
{
    private const ulong IndexMask = 0x_7FFF_FFFF;
    private const ulong VersionMask = 0x_7FFF_FFFF_0000_0000;

    public Id() : this(MinIndex) { }

    public Id(int index) : this(index, MinVersion) { }

    public Id(int index, int version)
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

    public bool Equals(Id other) =>
        Index == other.Index && Version == other.Version;

    public override bool Equals(object? obj) =>
        obj is Id id && id.Equals(this);

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

    /// <summary>
    /// Returns a 32-bit signed integer whose binary representation
    /// is the same as the first 4 bytes of <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The 64-bit unsigned integer to have its bytes reinterpreted.</param>
    /// <returns>The reinterpreted first 4 bytes of <paramref name="value"/>.</returns>
    /// <remarks>
    /// Reinterpreting is different to a simple cast in that the binary representation of the
    /// input value is treated as a different type without being altered.
    /// <para/>
    /// This method exists as the matching counterpart of <see cref="InterpretAsUInt64(int)"/>.
    /// </remarks>
    /// <seealso cref="InterpretAsUInt64(int)"/>
    private static unsafe int InterpretAsInt32(ulong value)
    {
        var span = new ReadOnlySpan<byte>(&value, sizeof(ulong));
        return BitConverter.ToInt32(span);
    }
    
    /// <summary>
    /// Returns a 64-bit unsigned integer whose binary representation
    /// of its first 4 bytes is the same as that of <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The 32-bit signed integer to have its bytes reinterpreted.</param>
    /// <returns>The reinterpreted bytes of <paramref name="value"/>.</returns>
    /// <remarks>
    /// Reinterpreting is different to a simple cast in that the binary representation of the
    /// input value is treated as a different type without being altered.
    /// <para/>
    /// This method exists as the matching counterpart of <see cref="InterpretAsInt32(int)"/>.
    /// </remarks>
    /// <seealso cref="InterpretAsInt32(int)"/>
    private static unsafe ulong InterpretAsUInt64(int value)
    {
        // BitConverter.ToUInt64() requires at least 8 bytes in the span.
        long longValue = value;
        var span = new ReadOnlySpan<byte>(&longValue, sizeof(long));
        return BitConverter.ToUInt64(span);
    }
}
