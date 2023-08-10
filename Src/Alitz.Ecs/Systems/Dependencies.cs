using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Alitz.Systems;
internal static class Dependencies
{
    public static Type[] GetSystemTypesOrderedByDependencies(IEnumerable<Type> systemTypes) =>
        MakeOrderedSystemTypeArray(MakeTree(MakeDependencyTable(systemTypes)));

    private static Type[] MakeOrderedSystemTypeArray(Dependency[] dependencyTree)
    {
        List<Type> orderedSystemTypes = new(dependencyTree.Length);
        foreach (var dependency in dependencyTree)
        {
            Visit(dependency, orderedSystemTypes, dependencyTree);
        }
        return orderedSystemTypes.ToArray();

        static void Visit(Dependency node, List<Type> types, Dependency[] dependencyTree)
        {
            if (types.Count >= dependencyTree.Length)
            {
                return;
            }
            foreach (var nestedNode in node.Dependencies)
            {
                Visit(nestedNode, types, dependencyTree);
            }
            if (!types.Contains(node.SystemType))
            {
                types.Add(node.SystemType);
            }
        }
    }

    private static Dictionary<Type, IEnumerable<Type>> MakeDependencyTable(IEnumerable<Type> systemTypes) =>
        systemTypes.Distinct()
            .ToDictionary(
                type => type,
                type => type.GetCustomAttributes<DependsOnAttribute>()
                    .Select(attribute => attribute.SystemType)
                    .Distinct());

    private static Dependency[] MakeTree(IReadOnlyDictionary<Type, IEnumerable<Type>> dependencyTable)
    {
        var treeNodes = CreateTopNodes(dependencyTable);
        bool hasExtendedAnyNodes = false;

        do
        {
            foreach (var treeNode in treeNodes)
            {
                if (TryExtendInnerNodes(treeNode, treeNodes))
                {
                    hasExtendedAnyNodes = true;
                }
                if (TryFindFirstNodeBeforeNodeWithSameInfo(treeNode, out var dependentNode))
                {
                    throw new CircularDependencyException(dependentNode.SystemType, treeNode.SystemType);
                }
            }
        }
        while (hasExtendedAnyNodes);

        return treeNodes.Select(MutableDependency.ToNode).ToArray();
    }

    private static MutableDependency[] CreateTopNodes(IReadOnlyDictionary<Type, IEnumerable<Type>> dependencyTable) =>
        dependencyTable.Select(
                entry => new MutableDependency(entry.Key)
                {
                    Dependencies = entry.Value.Select(type => new MutableDependency(type)).ToList(),
                })
            .ToArray();

    private static bool TryExtendInnerNodes(MutableDependency headNode, IEnumerable<MutableDependency> topNodes)
    {
        if (headNode.Dependencies.Any())
        {
            return headNode.Dependencies.Select(innerNode => TryExtendInnerNodes(innerNode, topNodes))
                .Aggregate(false, (left, right) => left || right);
        }

        var type = headNode.SystemType;
        var matchingTopNode = topNodes.Where(topNode => topNode.SystemType == type).Single();

        foreach (var innerNode in matchingTopNode.Dependencies)
        {
            headNode.Dependencies.Add(innerNode);
        }

        return headNode.Dependencies.Any();
    }

    private static bool TryFindFirstNodeBeforeNodeWithSameInfo(
        MutableDependency headNode,
        out MutableDependency result,
        MutableDependency? currentNode = null
    )
    {
        currentNode ??= headNode;
        foreach (var innerNode in currentNode.Value.Dependencies)
        {
            if (innerNode.SystemType.Equals(headNode.SystemType))
            {
                result = currentNode.Value;
                return true;
            }
            if (TryFindFirstNodeBeforeNodeWithSameInfo(headNode, out result, innerNode))
            {
                return true;
            }
        }
        result = default!;
        return false;
    }
}
