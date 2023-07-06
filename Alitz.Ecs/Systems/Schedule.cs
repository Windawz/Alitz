using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Alitz.Systems.Dependencies;

namespace Alitz.Systems;
internal static class Schedule
{
    public static IEnumerable<Type> Create(IEnvironment environment, IEnumerable<Type> thinkerTypes) =>
        MakeThinkerSchedule(
            Tree.Build(
                thinkerTypes.Distinct()
                    .ToDictionary(
                        type => type,
                        type => type.GetCustomAttributes<DependsOnAttribute>()
                            .Select(attribute => attribute.SystemType)
                            .Distinct())));

    public static IEnumerable<ISystem> Instantiate(IEnumerable<Type> schedule, IEnvironment environment) =>
        schedule.Select(type => Activator.CreateInstance(type)!).Cast<ISystem>();

    private static List<Type> MakeThinkerSchedule(Tree tree)
    {
        List<Type> types = new(tree.Nodes.Count);
        foreach (var node in tree.Nodes)
        {
            Visit(node, types, tree);
        }
        return types;

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
}
