using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Alitz.Ecs.Systems;
internal readonly struct SystemMetadata
{
    private SystemMetadata(Type systemType)
    {
        Systems.SystemType.ThrowIfNotValid(systemType, paramName: nameof(systemType));
        SystemType = systemType;
        Stage = SystemType
            .GetCustomAttribute<RunsAtStageAttribute>()?.Stage ?? default;
        Dependencies = SystemType
            .GetCustomAttributes<HasDependencyAttribute>()
            .Select(attribute => attribute.SystemType)
            .ToArray();
    }

    public Type SystemType { get; }
    public Stage Stage { get; }
    public IReadOnlyCollection<Type> Dependencies { get; }

    public static SystemMetadata Of(Type systemType) =>
        new(systemType);
}