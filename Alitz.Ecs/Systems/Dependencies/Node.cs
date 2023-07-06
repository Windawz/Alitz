using System;
using System.Collections.Generic;

namespace Alitz.Systems.Dependencies;
internal readonly record struct Node(Type SystemType, IReadOnlyList<Node> Nodes);
