using System;
using System.Collections.Generic;
using System.Linq;

namespace Alitz.Thinking.Dependencies;
internal readonly record struct MutableNode(Type ThinkerType)
{
    public IList<MutableNode> Nodes { get; init; } = new List<MutableNode>();

    public static Node ToNode(MutableNode mutableNode) =>
        new(mutableNode.ThinkerType, mutableNode.Nodes.Select(ToNode).ToArray());
}
