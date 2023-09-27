using System;

namespace Alitz.Ecs.Systems;
public class SystemType
{
    public SystemType(Type type)
    {
        if (!IsValid(type))
        {
            throw new ArgumentException(paramName: nameof(type), message: $"Type {type} does not implement {typeof(ISystem)}");
        }

        Type = type;
    }

    public Type Type { get; }

    public static implicit operator SystemType(Type type) =>
        new(type);

    public static bool operator==(SystemType left, SystemType right) =>
        left.Type.Equals(right.Type);

    public static bool operator!=(SystemType left, SystemType right) =>
        !left.Type.Equals(right.Type);

    public static bool operator==(SystemType left, Type right) =>
        left.Type.Equals(right);

    public static bool operator!=(SystemType left, Type right) =>
        !left.Type.Equals(right);

    public static bool operator==(Type left, SystemType right) =>
        left.Equals(right.Type);

    public static bool operator!=(Type left, SystemType right) =>
        !left.Equals(right.Type);

    public static bool IsValid(Type type) =>
        type.IsAssignableTo(typeof(ISystem));

    public override bool Equals(object? obj) =>
        Type.Equals(obj);

    public override int GetHashCode() =>
        Type.GetHashCode();

    public override string ToString() =>
        Type.ToString();
}