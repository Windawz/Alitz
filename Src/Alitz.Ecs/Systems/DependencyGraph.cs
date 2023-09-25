using System;
using System.Collections.Generic;
using System.Linq;

namespace Alitz.Ecs.Systems;
internal class DependencyGraph
{
    public DependencyGraph(IEnumerable<SystemType> systemTypes)
    {
        TopNodes = systemTypes.Select(systemType => MakeNode(SystemMetadata.Get(systemType))).ToArray();

        foreach (var circularDependency in EnumerateCircularDependencies(TopNodes))
        {
            throw new CircularDependencyException(
                dependent: circularDependency.Dependent.Type,
                dependency: circularDependency.Dependency.Type
            );
        }

        foreach (var incompatibleStage in EnumerateIncompatibleStages(TopNodes))
        {
            throw new IncompatibleStageException(
                dependent: incompatibleStage.Dependent.Type,
                dependentStage: incompatibleStage.DependentStage,
                dependency: incompatibleStage.Dependency.Type,
                dependencyStage: incompatibleStage.DependencyStage
            );
        }
    }

    public IReadOnlyCollection<DependencyNode> TopNodes { get; }

    private static DependencyNode MakeNode(SystemMetadata topMetadata, SystemMetadata? currentMetadata = null)
    {
        currentMetadata ??= topMetadata;
        if (currentMetadata.Dependencies.Count == 0)
        {
            return new DependencyNode(
                systemType: currentMetadata.SystemType,
                stage: currentMetadata.Stage,
                startsCircularDependency: false,
                Array.Empty<DependencyNode>()
            );
        }
        else
        {
            var childNodes = currentMetadata.Dependencies
                .Select(childSystemType =>
                    {
                        bool startsCircularDependency = childSystemType.Type == topMetadata.SystemType.Type;
                        var childMetadata = SystemMetadata.Get(childSystemType);
                        if (startsCircularDependency)
                        {
                            return new DependencyNode(
                                systemType: childMetadata.SystemType,
                                stage: childMetadata.Stage,
                                startsCircularDependency: true,
                                dependencies: Array.Empty<DependencyNode>()
                            );
                        }
                        else
                        {
                            return MakeNode(topMetadata, childMetadata);
                        }
                    }
                ).ToArray();

            return new DependencyNode(
                systemType: currentMetadata.SystemType,
                stage: currentMetadata.Stage,
                startsCircularDependency: false,
                dependencies: childNodes
            );
        }
    }

    private static IEnumerable<CircularDependencyInfo> EnumerateCircularDependencies(IReadOnlyCollection<DependencyNode> topNodes)
    {
        static IEnumerable<CircularDependencyInfo> Enumerate(DependencyNode topNode, DependencyNode? node = null)
        {
            node ??= topNode;
            if (node.StartsCircularDependency)
            {
                return Enumerable.Repeat(new CircularDependencyInfo(topNode.SystemType, node.SystemType), 1);
            }
            else
            {
                return node.Dependencies
                    .Select(childNode => Enumerate(topNode, childNode))
                    .Aggregate((left, right) => left.Concat(right));
            }
        }

        return topNodes.Select(topNode => Enumerate(topNode))
            .SelectMany(dependencies => dependencies)
            .ToArray();
    }

    private static IEnumerable<IncompatibleStageInfo> EnumerateIncompatibleStages(IReadOnlyCollection<DependencyNode> topNodes)
    {
        static IEnumerable<IncompatibleStageInfo> Enumerate(DependencyNode node)
        {
            foreach (var childNode in node.Dependencies)
            {
                if (childNode.Stage > node.Stage)
                {
                    yield return new IncompatibleStageInfo(
                        Dependent: node.SystemType,
                        DependentStage: node.Stage,
                        Dependency: childNode.SystemType,
                        DependencyStage: childNode.Stage
                    );
                }

                var childInfos = Enumerate(childNode);
                foreach (var childInfo in childInfos)
                {
                    yield return childInfo;
                }
            }
        }

        return topNodes.Select(topNode => Enumerate(topNode))
            .Aggregate((left, right) => left.Concat(right));
    }
}