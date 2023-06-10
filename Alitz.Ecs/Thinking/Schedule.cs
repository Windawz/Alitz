using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Alitz.Thinking.Dependencies;

namespace Alitz.Thinking;
internal static class Schedule
{
    public static IEnumerable<Type> Create(Environment environment, IEnumerable<Type> thinkerTypes) =>
        MakeThinkerSchedule(
            Tree.Build(
                thinkerTypes.Distinct()
                    .ToDictionary(
                        type => type,
                        type => type.GetCustomAttributes<DependsOnAttribute>()
                            .Select(attribute => attribute.ThinkerType)
                            .Distinct())));

    public static IEnumerable<Thinker> Instantiate(IEnumerable<Type> schedule, Environment environment) =>
        schedule.Select(type => Activator.CreateInstance(type, environment)!).Cast<Thinker>();

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
            if (!types.Contains(node.ThinkerType))
            {
                types.Add(node.ThinkerType);
            }
        }
    }
}
