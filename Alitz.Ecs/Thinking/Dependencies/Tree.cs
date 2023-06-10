using System;
using System.Collections.Generic;
using System.Linq;

namespace Alitz.Thinking.Dependencies;
internal class Tree
{
    private Tree(IReadOnlyList<Node> nodes)
    {
        Nodes = nodes;
    }

    public IReadOnlyList<Node> Nodes { get; }

    public static Tree Build(IReadOnlyDictionary<Type, IEnumerable<Type>> dependencyTable)
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
                    throw new CircularDependencyException(dependentNode.ThinkerType, treeNode.ThinkerType);
                }
            }
        }
        while (hasExtendedAnyNodes);

        return new Tree(treeNodes.Select(MutableNode.ToNode).ToArray());
    }

    private static MutableNode[] CreateTopNodes(IReadOnlyDictionary<Type, IEnumerable<Type>> dependencyTable) =>
        dependencyTable.Select(
                entry =>
                    new MutableNode(entry.Key) { Nodes = entry.Value.Select(type => new MutableNode(type)).ToList(), })
            .ToArray();

    private static bool TryExtendInnerNodes(MutableNode headNode, IEnumerable<MutableNode> topNodes)
    {
        if (headNode.Nodes.Any())
        {
            return headNode.Nodes.Select(innerNode => TryExtendInnerNodes(innerNode, topNodes))
                .Aggregate(false, (left, right) => left || right);
        }

        var type = headNode.ThinkerType;
        var matchingTopNode = topNodes.Where(topNode => topNode.ThinkerType == type).Single();

        foreach (var innerNode in matchingTopNode.Nodes)
        {
            headNode.Nodes.Add(innerNode);
        }

        return headNode.Nodes.Any();
    }

    private static bool TryFindFirstNodeBeforeNodeWithSameInfo(
        MutableNode headNode,
        out MutableNode result,
        MutableNode? currentNode = null
    )
    {
        currentNode ??= headNode;
        foreach (var innerNode in currentNode.Value.Nodes)
        {
            if (innerNode.ThinkerType.Equals(headNode.ThinkerType))
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
