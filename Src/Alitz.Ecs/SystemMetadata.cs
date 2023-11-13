using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Alitz.Ecs;
internal readonly struct SystemMetadata
{
    public SystemMetadata(Type systemType)
    {
        SystemType.ThrowIfNotValid(systemType, paramName: nameof(systemType));
        Stage = systemType
            .GetCustomAttribute<RunsAtStageAttribute>()?.Stage ?? default;
        Dependencies = systemType
            .GetCustomAttributes<HasDependencyAttribute>()
            .Select(attribute => attribute.SystemType)
            .ToArray();
    }

    public Stage Stage { get; }
    public IReadOnlyCollection<Type> Dependencies { get; }
}