using System;
using System.Diagnostics.CodeAnalysis;

namespace Alitz.Ecs;
public readonly struct Stage : IComparable<Stage>, IEquatable<Stage>
{
    public Stage(int number)
    {
        Number = number;
    }

    public int Number { get; }

    public static bool operator>(Stage left, Stage right) =>
        left.CompareTo(right) > 0;

    public static bool operator>=(Stage left, Stage right) =>
        left.CompareTo(right) >= 0;

    public static bool operator==(Stage left, Stage right) =>
        left.Equals(right);

    public static bool operator!=(Stage left, Stage right) =>
        !left.Equals(right);

    public static bool operator<(Stage left, Stage right) =>
        left.CompareTo(right) < 0;

    public static bool operator<=(Stage left, Stage right) =>
        left.CompareTo(right) <= 0;

    public int CompareTo(Stage other) =>
        Number.CompareTo(other.Number);

    public bool Equals(Stage other) =>
        Number.Equals(other.Number);

    public override bool Equals([NotNullWhen(true)] object? obj) => obj switch
    {
        null => false,
        Stage other => Equals(other),
        _ => false,
    };

    public override int GetHashCode() =>
        Number.GetHashCode();
}