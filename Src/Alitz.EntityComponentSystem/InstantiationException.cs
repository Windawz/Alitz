using System;

namespace Alitz.EntityComponentSystem;
public class InstantiationException : EcsException
{
    public InstantiationException() { }

    public InstantiationException(string message) : base(message) { }

    public InstantiationException(string message, Exception inner) : base(message, inner) { }
}