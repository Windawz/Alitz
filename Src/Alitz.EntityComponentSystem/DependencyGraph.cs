using System;
using System.Collections.Generic;
using System.Linq;

using Alitz.Common.Collections;

namespace Alitz.EntityComponentSystem;
internal class DependencyGraph : IGraph<DependencyInfo>
{
    public DependencyGraph(Type systemType)
    {
        SystemType.ThrowIfNotValid(systemType, paramName: nameof(systemType));

        var graph = MakeGraph(systemType);
        (Value, Children) = (graph.Value, graph.Children);
    }

    // Used while building the graph
    private DependencyGraph(DependencyInfo dependencyInfo, IReadOnlyCollection<DependencyGraph> dependencies)
    {
        Value = dependencyInfo;
        Children = dependencies.ToArray();
    }

    public DependencyInfo Value { get; }

    DependencyInfo IGraph<DependencyInfo>.Value =>
        Value;

    public IReadOnlyCollection<DependencyGraph> Children { get; }

    IReadOnlyCollection<IGraph<DependencyInfo>> IGraph<DependencyInfo>.Children =>
        Children;

    private static DependencyGraph MakeGraph(Type topType, Type? currentType = null)
    {
        currentType ??= topType;

        var currentMetadata = new SystemMetadata(currentType);

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
                        if (dependencyType == topType)
                        {
                            var dependencyMetadata = new SystemMetadata(dependencyType);
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
                    SystemType: currentType,
                    Stage: currentMetadata.Stage,
                    StartsCircularDependency: false
                ),
                dependencies: childNodes
            );
        }
    }
}