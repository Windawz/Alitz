using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Alitz.Ecs.Systems;
internal readonly struct SystemMetadata
{
    private SystemMetadata(SystemType systemType)
    {
        SystemType = systemType;
        Stage = SystemType
            .Type
            .GetCustomAttribute<RunsAtStageAttribute>()?.Stage ?? default;
        Dependencies = SystemType
            .Type
            .GetCustomAttributes<HasDependencyAttribute>()
            .Select(attribute => new SystemType(attribute.SystemType))
            .ToArray();
    }

    public SystemType SystemType { get; }
    public Stage Stage { get; }
    public IReadOnlyCollection<SystemType> Dependencies { get; }

    public static SystemMetadata Of(SystemType systemType) =>
        new(systemType);
}