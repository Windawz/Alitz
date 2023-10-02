using System.Collections.Generic;

namespace Alitz.Common.Collections;
public static class GraphExtensions
{
    public static IEnumerable<IGraph<T>> EnumerateBroad<T>(this IGraph<T> graph)
    {
        var queue = new Queue<IGraph<T>>();
        var visited = new HashSet<IGraph<T>>();
        queue.Enqueue(graph);
        while (queue.TryDequeue(out var node))
        {
            visited.Add(node);
            yield return node;
            foreach (var child in node.Children)
            {
                if (!visited.Contains(child))
                {
                    queue.Enqueue(child);
                }
            }
        }
    }

    public static IEnumerable<IGraph<T>> EnumerateDeep<T>(this IGraph<T> graph)
    {
        static IEnumerable<IGraph<T>> Enumerate(IGraph<T> graph, ISet<IGraph<T>> visited)
        {
            yield return graph;
            foreach (var node in graph.Children)
            {
                if (!visited.Contains(node))
                {
                    visited.Add(node);
                    foreach (var child in Enumerate(node, visited))
                    {
                        yield return child;
                    }
                }
            }
        }

        var visited = new HashSet<IGraph<T>>();
        return Enumerate(graph, visited);
    }
}