using System;
using System.Collections.Generic;
using System.Linq;

namespace Alitz.Ecs.Systems.Scheduling;
internal readonly record struct MutableDependency(Type SystemType)
{
    public IList<MutableDependency> Dependencies { get; init; } = new List<MutableDependency>();

    public static Dependency ToNode(MutableDependency mutableNode) =>
        new(mutableNode.SystemType, mutableNode.Dependencies.Select(ToNode).ToArray());
}
