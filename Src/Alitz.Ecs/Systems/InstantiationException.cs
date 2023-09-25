using System;

namespace Alitz.Ecs.Systems;
public class InstantiationException : EcsException
{
    public InstantiationException() { }

    public InstantiationException(string message) : base(message) { }

    public InstantiationException(string message, Exception inner) : base(message, inner) { }
}