using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Alitz.Ecs.Systems;
internal class SystemSchedule : IReadOnlyCollection<SystemType>
{
    public SystemSchedule(DependencyGraph graph)
    {
        var systemTypeListsPerStage = new Dictionary<Stage, List<SystemType>>();
        var queue = new Queue<DependencyNode>();
        foreach (var node in graph.TopNodes)
        {
            queue.Enqueue(node);
        }
        while (queue.TryDequeue(out var node))
        {
            if (!systemTypeListsPerStage.ContainsKey(node.Stage))
            {
                systemTypeListsPerStage[node.Stage] = new List<SystemType>();
            }
            systemTypeListsPerStage[node.Stage].Add(node.SystemType);
            foreach (var childNode in node.Dependencies)
            {
                queue.Enqueue(childNode);
            }
        }

        var systemTypeCount = systemTypeListsPerStage
            .Sum(systemTypes => systemTypes.Value.Count);
        var orderedSystemTypes = new List<SystemType>(systemTypeCount);
        foreach (var (_, systemTypes) in systemTypeListsPerStage.OrderBy(kv => kv.Key))
        {
            orderedSystemTypes.AddRange(systemTypes);
        }
        _systemTypes = orderedSystemTypes;
    }

    private IReadOnlyCollection<SystemType> _systemTypes;

    public int Count =>
        _systemTypes.Count;

    public IEnumerator<SystemType> GetEnumerator() =>
        _systemTypes.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() =>
        GetEnumerator();
}