using System;
using System.Collections.Generic;

namespace Alitz.Thinking.Dependencies;
internal readonly record struct Node(Type ThinkerType, IReadOnlyList<Node> Nodes);
