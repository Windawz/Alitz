using System;

namespace Alitz.Ecs.Systems;
public static class SystemType
{
    public static bool IsValid(Type systemType) =>
        systemType.IsAssignableTo(typeof(ISystem));

    internal static void ThrowIfNotValid(Type systemType, string? message = null, string? paramName = null)
    {
        if (!IsValid(systemType))
        {
            throw new ArgumentException(
                message: message ?? $"Type {systemType.FullName} is not a system",
                paramName: paramName
            );
        }
    }
}