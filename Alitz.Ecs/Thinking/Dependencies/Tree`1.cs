using System.Collections.Generic;
using System.Linq;

namespace Alitz.Thinking.Dependencies;
internal class Tree<T> where T : notnull
{
    internal Tree(IEnumerable<Node<T>> topNodes)
    {
        TopNodes = topNodes.ToArray();
    }

    public IEnumerable<Node<T>> TopNodes { get; }
}
