using System.Collections.Generic;
using System.Linq;

namespace Alitz.Thinking.Dependencies;
internal static class Tree
{
    public static Tree<T> Build<T>(IReadOnlyDictionary<T, IReadOnlyCollection<T>> dependencyTable) where T : notnull
    {
        var topNodes = CreateTopNodes(dependencyTable);
        bool hasExtendedAnyNodes;

        do
        {
            hasExtendedAnyNodes = topNodes.Select(topNode => ExtendInnerNodesOnce(topNode, topNodes))
                .Where(result => result)
                .Any();

            foreach (var topNode in topNodes)
            {
                if (TryFindFirstNodeBeforeNodeWithSameInfo(topNode, out var dependentNode))
                {
                    throw new CircularDependencyException<T>(dependentNode.Value, topNode.Value);
                }
            }
        }
        while (hasExtendedAnyNodes);

        return new Tree<T>(topNodes.Select(MutableNode<T>.ToNode).ToArray());
    }

    private static bool ExtendInnerNodesOnce<T>(MutableNode<T> headNode, IReadOnlyCollection<MutableNode<T>> topNodes)
        where T : notnull
    {
        if (headNode.Nodes.Any())
        {
            return headNode.Nodes.Select(innerNode => ExtendInnerNodesOnce(innerNode, topNodes))
                .Where(result => result)
                .Any();
        }

        var info = headNode.Value;
        var matchingTopNode = topNodes.Where(topNode => topNode.Value.Equals(info)).Single();

        foreach (var innerNode in matchingTopNode.Nodes)
        {
            headNode.Nodes.Add(innerNode);
        }

        return headNode.Nodes.Any();
    }

    private static bool TryFindFirstNodeBeforeNodeWithSameInfo<T>(MutableNode<T> headNode, out MutableNode<T> result)
        where T : notnull
    {
        static bool TryFindFirstNodeBeforeNodeWithSameInfoInner(
            MutableNode<T> headNode,
            out MutableNode<T> result,
            MutableNode<T>? currentNode = null
        )
        {
            currentNode ??= headNode;
            foreach (var innerNode in currentNode.Value.Nodes)
            {
                if (innerNode.Value.Equals(headNode.Value))
                {
                    result = currentNode.Value;
                    return true;
                }
                if (TryFindFirstNodeBeforeNodeWithSameInfoInner(headNode, out result, innerNode))
                {
                    return true;
                }
            }
            result = default!;
            return false;
        }

        return TryFindFirstNodeBeforeNodeWithSameInfoInner(headNode, out result);
    }

    private static MutableNode<T>[] CreateTopNodes<T>(IReadOnlyDictionary<T, IReadOnlyCollection<T>> dependencyTable)
        where T : notnull
    {
        Dictionary<T, MutableNode<T>> topNodes = new();
        foreach (var (info, dependencies) in dependencyTable)
        {
            topNodes.TryAdd(
                info,
                new MutableNode<T>(info)
                {
                    Nodes = dependencies.Select(dependency => new MutableNode<T>(dependency)).ToList(),
                });
        }
        foreach (var (_, dependencies) in dependencyTable)
        {
            foreach (var dependency in dependencies)
            {
                topNodes.TryAdd(dependency, new MutableNode<T>(dependency));
            }
        }
        return topNodes.Values.ToArray();
    }
}
