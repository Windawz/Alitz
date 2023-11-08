using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Alitz.Common.Collections;

namespace Alitz.Ecs.Systems;
public class SystemSchedule : IReadOnlyCollection<Type>
{
    public SystemSchedule(IEnumerable<Type> systemTypes)
    {
        _systemTypes = systemTypes
            .Select(systemType =>
            {
                SystemType.ThrowIfNotValid(systemType);
                return new DependencyGraph(systemType);
            })
            .SelectMany(
                // We need to connect together all the graphs into one single
                // enumerable.
                //
                // Going breadth-first from end to start of the first graph,
                // then from end to start of the next graph, and so on.
                //
                // Starting from the end is necessary because systems
                // with no dependencies must be first in the scedule.
                graph => graph.EnumerateBroad()
                    .Reverse()
                    .Select(graph => graph.Value)
            )
            .GroupBy(info => info.Stage.Number)
            .OrderBy(group => group.Key)
            .SelectMany(group => group)
            .Select(info => info.SystemType)
            // Possible duplicates because each system type had
            // its dependency graph made in isolation from others.
            .Distinct()
            .ToArray();
    }

    private IReadOnlyCollection<Type> _systemTypes;

    public int Count =>
        _systemTypes.Count;

    public IEnumerator<Type> GetEnumerator() =>
        _systemTypes.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() =>
        GetEnumerator();
}