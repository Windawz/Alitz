using System;

namespace Alitz.Ecs.Systems;
public class SystemType
{
    public SystemType(Type type)
    {
        if (!type.IsAssignableTo(typeof(ISystem)))
        {
            throw new ArgumentException(paramName: nameof(type), message: $"Type {type} does not implement {typeof(ISystem)}");
        }

        Type = type;
    }

    public Type Type { get; }

    public static implicit operator SystemType(Type type) =>
        new(type);

    public static implicit operator Type(SystemType systemType) =>
        systemType.Type;
}