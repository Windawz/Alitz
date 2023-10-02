using System;
using System.Collections.Generic;
using System.Linq;

using Alitz.Common.Collections;

namespace Alitz.Ecs.Systems;
internal class DependencyGraph : IGraph< DependencyInfo>
{
    private DependencyGraph(DependencyInfo dependencyInfo, IEnumerable<DependencyGraph> dependencies)
    {
        DependencyInfo = dependencyInfo;
        Dependencies = dependencies.ToArray();
    }

    public DependencyInfo DependencyInfo { get; }

    DependencyInfo IGraph<DependencyInfo>.Value =>
        DependencyInfo;

    public IReadOnlyCollection<DependencyGraph> Dependencies { get; }

    IReadOnlyCollection<IGraph<DependencyInfo>> IGraph<DependencyInfo>.Children =>
        Dependencies;

    public static DependencyGraph Build(SystemType systemType)
    {
        var graph = MakeGraph(systemType);

        foreach (var circularDependency in EnumerateCircularDependencies(graph))
        {
            throw new CircularDependencyException(
                dependent: circularDependency.Dependent.Type,
                dependency: circularDependency.Dependency.Type
            );
        }

        foreach (var incompatibleStage in EnumerateIncompatibleStages(graph))
        {
            throw new IncompatibleStageException(
                dependent: incompatibleStage.Dependent.Type,
                dependentStage: incompatibleStage.DependentStage,
                dependency: incompatibleStage.Dependency.Type,
                dependencyStage: incompatibleStage.DependencyStage
            );
        }

        return graph;
    }

    private static DependencyGraph MakeGraph(SystemType topType, SystemType? currentType = null)
    {
        currentType ??= topType;

        var currentMetadata = SystemMetadata.Of(currentType);

        if (currentMetadata.Dependencies.Count == 0)
        {
            return new DependencyGraph(
                new DependencyInfo(
                    SystemType: currentType,
                    Stage: currentMetadata.Stage,
                    StartsCircularDependency: false
                ),
                Array.Empty<DependencyGraph>()
            );
        }
        else
        {
            var childNodes = currentMetadata.Dependencies
                .Select(dependencyType =>
                    {
                        if (dependencyType.Type == topType.Type)
                        {
                            var dependencyMetadata = SystemMetadata.Of(dependencyType.Type);
                            return new DependencyGraph(
                                new DependencyInfo(
                                    SystemType: dependencyType,
                                    Stage: dependencyMetadata.Stage,
                                    StartsCircularDependency: true
                                ),
                                dependencies: Array.Empty<DependencyGraph>()
                            );
                        }
                        else
                        {
                            return MakeGraph(topType, dependencyType);
                        }
                    }
                ).ToArray();

            return new DependencyGraph(
                new DependencyInfo(
                    SystemType: currentType.Type,
                    Stage: currentMetadata.Stage,
                    StartsCircularDependency: false
                ),
                dependencies: childNodes
            );
        }
    }

    private static IEnumerable<CircularDependencyInfo> EnumerateCircularDependencies(DependencyGraph graph)
    {
        static IEnumerable<CircularDependencyInfo> Enumerate(DependencyGraph top, DependencyGraph? node = null)
        {
            node ??= top;
            if (node.DependencyInfo.StartsCircularDependency)
            {
                return Enumerable.Repeat(
                    new CircularDependencyInfo(
                        top.DependencyInfo.SystemType,
                        node.DependencyInfo.SystemType
                    ),
                    1
                );
            }
            else
            {
                return node.Dependencies
                    .Select(childNode => Enumerate(top, childNode))
                    .Aggregate(Enumerable.Empty<CircularDependencyInfo>(), (left, right) => left.Concat(right));
            }
        }

        return Enumerate(graph);
    }

    private static IEnumerable<IncompatibleStageInfo> EnumerateIncompatibleStages(DependencyGraph graph)
    {
        static IEnumerable<IncompatibleStageInfo> Enumerate(DependencyGraph node)
        {
            foreach (var child in node.Dependencies)
            {
                if (child.DependencyInfo.Stage > node.DependencyInfo.Stage)
                {
                    yield return new IncompatibleStageInfo(
                        Dependent: node.DependencyInfo.SystemType,
                        DependentStage: node.DependencyInfo.Stage,
                        Dependency: child.DependencyInfo.SystemType,
                        DependencyStage: child.DependencyInfo.Stage
                    );
                }

                var childInfos = Enumerate(child);
                foreach (var info in childInfos)
                {
                    yield return info;
                }
            }
        }

        return Enumerate(graph);
    }
}