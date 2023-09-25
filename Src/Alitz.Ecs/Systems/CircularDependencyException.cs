using System;

namespace Alitz.Ecs.Systems;
public class CircularDependencyException : DependencyException
{
    public CircularDependencyException(Type dependent, Type dependency)
    {
        Dependent = dependent;
        Dependency = dependency;
    }

    public Type Dependent { get; }
    public Type Dependency { get; }

    /// <inheritdoc />
    public override string Message =>
        "Circular dependency detected between dependent "
        + Dependent.FullName
        + " and its dependency "
        + Dependency.FullName;
}
