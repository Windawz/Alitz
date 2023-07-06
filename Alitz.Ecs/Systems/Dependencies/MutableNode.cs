using System;
using System.Collections.Generic;
using System.Linq;

namespace Alitz.Systems.Dependencies;
internal readonly record struct MutableNode(Type SystemType)
{
    public IList<MutableNode> Nodes { get; init; } = new List<MutableNode>();

    public static Node ToNode(MutableNode mutableNode) =>
        new(mutableNode.SystemType, mutableNode.Nodes.Select(ToNode).ToArray());
}
