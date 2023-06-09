using System;

namespace Alitz.Thinking.Dependencies;
public class CircularDependencyException<T> : Exception
{
    public CircularDependencyException(T dependent, T dependency)
    {
        Dependent = dependent;
        Dependency = dependency;
    }

    public T Dependent { get; }
    public T Dependency { get; }

    /// <inheritdoc />
    public override string Message =>
        "Circular dependency detected between dependent "
        + Dependent.GetType().FullName
        + $"and dependency {Dependency.GetType().FullName}";
}
