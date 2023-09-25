using System.Collections.Generic;

namespace Alitz.Ecs.Systems;
internal class DependencyNode
{
    public DependencyNode(SystemType systemType, Stage stage, bool startsCircularDependency, IReadOnlyCollection<DependencyNode> dependencies)
    {
        SystemType = systemType;
        Stage = stage;
        StartsCircularDependency = startsCircularDependency;
        Dependencies = dependencies;
    }

    public SystemType SystemType { get; }
    public Stage Stage { get; }
    public bool StartsCircularDependency { get; }
    public IReadOnlyCollection<DependencyNode> Dependencies { get; }
}