using System;
using System.Collections.Generic;

namespace Alitz.Thinking;
internal partial class Configuration
{
    internal Configuration(Type thinkerType, IReadOnlyList<Type> dependencies)
    {
        ThinkerType = thinkerType;
        Dependencies = dependencies;
    }

    public Type ThinkerType { get; }
    public IReadOnlyList<Type> Dependencies { get; }
}
