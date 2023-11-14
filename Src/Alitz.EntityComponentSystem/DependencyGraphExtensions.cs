using System.Collections.Generic;
using System.Linq;

using Alitz.Common.Collections;

namespace Alitz.EntityComponentSystem;
public static class DependencyGraphExtensions
{
    public static IEnumerable<CircularDependencyInfo> EnumerateCircularDependencies(this IGraph<DependencyInfo> graph)
    {
        static IEnumerable<CircularDependencyInfo> Enumerate(IGraph<DependencyInfo> top, IGraph<DependencyInfo>? node = null)
        {
            node ??= top;
            if (node.Value.StartsCircularDependency)
            {
                return Enumerable.Repeat(
                    new CircularDependencyInfo(
                        top.Value.SystemType,
                        node.Value.SystemType
                    ),
                    1
                );
            }
            else
            {
                return node.Children
                    .Select(childNode => Enumerate(top, childNode))
                    .Aggregate(Enumerable.Empty<CircularDependencyInfo>(), (left, right) => left.Concat(right));
            }
        }

        return Enumerate(graph);
    }

    public static IEnumerable<IncompatibleStageInfo> EnumerateIncompatibleStages(this IGraph<DependencyInfo> graph)
    {
        static IEnumerable<IncompatibleStageInfo> Enumerate(IGraph<DependencyInfo> node)
        {
            foreach (var child in node.Children)
            {
                if (child.Value.Stage > node.Value.Stage)
                {
                    yield return new IncompatibleStageInfo(
                        Dependent: node.Value.SystemType,
                        DependentStage: node.Value.Stage,
                        Dependency: child.Value.SystemType,
                        DependencyStage: child.Value.Stage
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