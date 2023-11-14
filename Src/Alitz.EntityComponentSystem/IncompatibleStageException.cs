using System;

namespace Alitz.EntityComponentSystem;
public class IncompatibleStageException : DependencyException
{
    public IncompatibleStageException(Type dependent, Stage dependentStage, Type dependency, Stage dependencyStage)
    {
        Dependent = dependent;
        DependentStage = dependentStage;
        Dependency = dependency;
        DependencyStage = dependencyStage;
    }

    public Type Dependent { get; }
    public Stage DependentStage { get; }
    public Type Dependency { get; }
    public Stage DependencyStage { get; }

    /// <inheritdoc />
    public override string Message =>
        "System of type "
        + Dependent
        + " depends on system of type "
        + Dependency
        + " that runs at a later stage";
}
