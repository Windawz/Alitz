using System;
using System.Numerics;

namespace Alitz.Ecs;
public readonly struct Entity : IEquatable<Entity> {
    public Entity() {
        ulong data = 0;
        SetId(ref data, NullId);
        SetVersion(ref data, 0);
        _data = data;
    }

    public Entity(int id) : this(id, 0) { }

    public Entity(int id, int version) {
        if (id > IdMaxValue) {
            throw new ArgumentOutOfRangeException(nameof(id));
        }
        if (version > VersionMaxValue) {
            throw new ArgumentOutOfRangeException(nameof(version));
        }
        ulong data = 0;
        SetId(ref data, InterpretAsUInt64(id));
        SetVersion(ref data, InterpretAsUInt64(version));
        _data = data;
    }

    public const int IdMinValue = 0;
    public const int VersionMinValue = 0;
    public static readonly int IdMaxValue = InterpretAsInt32(IdMask - 1);
    public static readonly int VersionMaxValue = InterpretAsInt32(
        VersionMask >> BitOperations.TrailingZeroCount(VersionMask)
    );
    private const ulong NullId = IdMask;
    private const ulong IdMask = 0x_FFFF_FFFF;
    private const ulong VersionMask = 0x_FFFF_FFFF_0000_0000;

    private readonly ulong _data;

    public static Entity Null { get; } = new();

    public int Id =>
        InterpretAsInt32(GetId(_data));

    public int Version =>
        InterpretAsInt32(GetVersion(_data));

    public static bool operator ==(Entity lhs, Entity rhs) =>
        lhs.Equals(rhs);

    public static bool operator !=(Entity lhs, Entity rhs) =>
        !lhs.Equals(rhs);

    public static void ThrowIfNull(
        Entity entity,
        string? paramName = null,
        string? message = null
    ) {
        if (entity == Null) {
            throw new NullEntityException(
                message ?? "Entity may not be null",
                paramName ?? nameof(entity)
            );
        }
    }

    public bool Equals(Entity other) =>
        Id == other.Id && (IsIdNull(_data) || Version == other.Version);

    public override bool Equals(object? obj) =>
        obj is Entity other && Equals(other);

    public override int GetHashCode() =>
        IsIdNull(_data)
            ? GetId(_data).GetHashCode()
            : _data.GetHashCode();

    private static bool IsIdNull(ulong data) =>
        GetId(data) == NullId;

    private static ulong GetId(ulong data) =>
        data & IdMask;

    private static ulong GetVersion(ulong data) =>
        (data & VersionMask) >> 32;

    private static void SetId(ref ulong data, ulong id) =>
        data |= id & IdMask;

    private static void SetVersion(ref ulong data, ulong version) =>
        data |= version << 32 & VersionMask;

    private static unsafe int InterpretAsInt32(ulong value) {
        var span = new ReadOnlySpan<byte>(&value, sizeof(ulong));
        return BitConverter.ToInt32(span);
    }

    private static unsafe ulong InterpretAsUInt64(int value) {
        var span = new ReadOnlySpan<byte>(&value, sizeof(int));
        return BitConverter.ToUInt64(span);
    }
}
