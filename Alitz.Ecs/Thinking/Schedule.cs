using System;
using System.Collections.Generic;
using System.Linq;

using Alitz.Thinking.Dependencies;

namespace Alitz.Thinking;
internal static class Schedule
{
    public static IEnumerable<Thinker> Create(Environment environment, IEnumerable<Type> thinkerTypes)
    {
        var thinkerTable = MakeThinkerTable(environment, thinkerTypes);
        var configurationTable = MakeConfigurationTable(thinkerTable);
        var dependencyTable = MakeDependencyTable(thinkerTable, configurationTable);
        var dependencyTree = Tree.Build(dependencyTable);
        return MakeThinkerSchedule(dependencyTree, dependencyTable.Count);
    }

    private static Dictionary<Type, Thinker> MakeThinkerTable(Environment environment, IEnumerable<Type> thinkerTypes) =>
        thinkerTypes.Distinct()
            .ToDictionary(thinkerType => thinkerType, thinkerType => Instantiate(environment, thinkerType));

    private static Dictionary<Type, Configuration> MakeConfigurationTable(
        IReadOnlyDictionary<Type, Thinker> thinkerTable
    ) =>
        thinkerTable.ToDictionary(kv => kv.Key, kv => GetConfiguration(kv.Value));

    private static Dictionary<Thinker, IReadOnlyCollection<Thinker>> MakeDependencyTable(
        IReadOnlyDictionary<Type, Thinker> thinkerTable,
        IReadOnlyDictionary<Type, Configuration> configurationTable
    ) =>
        thinkerTable.Join(
                configurationTable,
                thinkerTableKeyValue => thinkerTableKeyValue.Key,
                configurationTableKeyValue => configurationTableKeyValue.Key,
                (thinkerTableKeyValue, configurationTableKeyValue) =>
                {
                    var thinker = thinkerTableKeyValue.Value;
                    var dependencyTypes = configurationTableKeyValue.Value.Dependencies;
                    IReadOnlyCollection<Thinker> thinkers = dependencyTypes
                        .Select(dependencyType => thinkerTable[dependencyType])
                        .ToArray();
                    return (thinker, thinkers);
                })
            .ToDictionary(tuple => tuple.thinker, value => value.thinkers);

    private static List<Thinker> MakeThinkerSchedule(Tree<Thinker> dependencyTree, int thinkerCount)
    {
        List<Thinker> schedule = new(thinkerCount);

        void Visit(Node<Thinker> node)
        {
            if (schedule.Count >= thinkerCount)
            {
                return;
            }
            foreach (var nestedNode in node.Nodes)
            {
                Visit(nestedNode);
            }
            if (!schedule.Contains(node.Value))
            {
                schedule.Add(node.Value);
            }
        }

        foreach (var node in dependencyTree.TopNodes)
        {
            Visit(node);
        }

        return schedule;
    }

    private static Configuration GetConfiguration(Thinker thinker)
    {
        var builder = new Configuration.Builder();
        thinker.Configure(builder);
        return builder.Build();
    }

    private static Thinker Instantiate(Environment environment, Type thinkerType) =>
        (Thinker)Activator.CreateInstance(thinkerType, environment)!;
}
