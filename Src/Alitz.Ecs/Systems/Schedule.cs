using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Alitz.Systems.Dependencies;

namespace Alitz.Systems;
internal class Schedule
{
    public Schedule(IEnumerable<SystemFactory> factories)
    {
        var distinctfactoryArray = factories.DistinctBy(factory => factory.SystemType).ToArray();
        var dependencyTree = MakeDependencyTree(distinctfactoryArray.Select(factory => factory.SystemType));
        _systems = MakeOrderedSystemTypeArray(dependencyTree)
            .Join(
                distinctfactoryArray,
                orderedSystemType => orderedSystemType,
                factory => factory.SystemType,
                (_, factory) => factory)
            .Select(factory => factory.Create())
            .ToArray();
    }

    private readonly ISystem[] _systems;

    public void Update(ISystemContext context, long elapsedMilliseconds)
    {
        for (int i = 0; i < _systems.Length; i++)
        {
            _systems[i].Update(context, elapsedMilliseconds);
        }
    }

    private static Type[] MakeOrderedSystemTypeArray(Tree dependencyTree)
    {
        List<Type> types = new(dependencyTree.Nodes.Count);
        foreach (var node in dependencyTree.Nodes)
        {
            Visit(node, types, dependencyTree);
        }
        return types.ToArray();

        static void Visit(Node node, List<Type> types, Tree tree)
        {
            if (types.Count >= tree.Nodes.Count)
            {
                return;
            }
            foreach (var nestedNode in node.Nodes)
            {
                Visit(nestedNode, types, tree);
            }
            if (!types.Contains(node.SystemType))
            {
                types.Add(node.SystemType);
            }
        }
    }

    private static Tree MakeDependencyTree(IEnumerable<Type> systemTypes) =>
        Tree.Build(
            systemTypes.Distinct()
                .ToDictionary(
                    type => type,
                    type => type.GetCustomAttributes<DependsOnAttribute>()
                        .Select(attribute => attribute.SystemType)
                        .Distinct()));
}
