using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Alitz.Systems.Dependencies;

namespace Alitz.Systems;
public class Schedule
{
    public Schedule(Type[] systemTypes)
    {
        foreach (var type in systemTypes)
        {
            if (!type.IsAssignableTo(typeof(ISystem)))
            {
                throw new ArgumentException($"Type {type} is not a system", nameof(systemTypes));
            }
        }
        _systems = Instantiate(MakeOrderedSystemTypeArray(MakeDependencyTree(systemTypes)));
    }

    private readonly ISystem[] _systems;

    public void Update(ISystemContext context, double delta)
    {
        for (int i = 0; i < _systems.Length; i++)
        {
            _systems[i].Update(context, delta);
        }
    }

    private static ISystem[] Instantiate(Type[] systemTypes)
    {
        var systems = new ISystem[systemTypes.Length];
        for (int i = 0; i < systemTypes.Length; i++)
        {
            systems[i] = (ISystem)Activator.CreateInstance(systemTypes[i])!;
        }
        return systems;
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
