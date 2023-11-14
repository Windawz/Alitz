using System;

namespace Alitz.EntityComponentSystem;
public class DependencyException : EcsException
{
    public DependencyException() { }

    public DependencyException(string message) : base(message) { }

    public DependencyException(string message, Exception inner) : base(message, inner) { }
}
