using System.Collections.Generic;
using System.Linq;

namespace Alitz.Thinking.Dependencies;
internal readonly record struct Node<T>(T Value, IReadOnlyCollection<Node<T>> Nodes) where T : notnull
{
    public override string ToString()
    {
        string nodes = string.Join(", ", Nodes.Select(inner => inner.ToString()));
        return $"{Value}({nodes})";
    }
}
