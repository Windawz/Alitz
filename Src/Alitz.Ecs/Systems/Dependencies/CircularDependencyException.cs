using System;

namespace Alitz.Systems.Dependencies;
public class CircularDependencyException : EcsException
{
    public CircularDependencyException(Type dependentSystemType, Type dependencySystemType)
    {
        DependentSystemType = dependentSystemType;
        DependencySystemType = dependencySystemType;
    }

    public Type DependentSystemType { get; }
    public Type DependencySystemType { get; }

    /// <inheritdoc />
    public override string Message =>
        $"Circular dependency detected between dependent {nameof(ISystem)} "
        + DependentSystemType.FullName
        + $" and its dependency {nameof(ISystem)} "
        + DependencySystemType.FullName;
}
