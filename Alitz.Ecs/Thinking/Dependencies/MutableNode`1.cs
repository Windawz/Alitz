using System.Collections.Generic;
using System.Linq;

namespace Alitz.Thinking.Dependencies;
internal readonly record struct MutableNode<T>(T Value) where T : notnull
{
    public ICollection<MutableNode<T>> Nodes { get; init; } = new List<MutableNode<T>>();

    public static Node<T> ToNode(MutableNode<T> mutableNode) =>
        new(mutableNode.Value, mutableNode.Nodes.Select(ToNode).ToArray());
}
