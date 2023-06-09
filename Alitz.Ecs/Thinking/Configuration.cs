using System;
using System.Collections.Generic;

namespace Alitz.Thinking;
internal readonly partial struct Configuration
{
    internal Configuration(IReadOnlyList<Type> dependencies)
    {
        Dependencies = dependencies;
    }

    public IReadOnlyList<Type> Dependencies { get; }
}
