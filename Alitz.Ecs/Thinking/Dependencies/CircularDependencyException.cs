using System;

namespace Alitz.Thinking.Dependencies;
public class CircularDependencyException : Exception
{
    public CircularDependencyException(Type dependentThinkerType, Type dependencyThinkerType)
    {
        DependentThinkerType = dependentThinkerType;
        DependencyThinkerType = dependencyThinkerType;
    }

    public Type DependentThinkerType { get; }
    public Type DependencyThinkerType { get; }

    /// <inheritdoc />
    public override string Message =>
        $"Circular dependency detected between dependent {nameof(Thinker)} "
        + DependentThinkerType.FullName
        + $" and its dependency {nameof(Thinker)} "
        + DependencyThinkerType.FullName;
}
