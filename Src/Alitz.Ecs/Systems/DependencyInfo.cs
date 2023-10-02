namespace Alitz.Ecs.Systems;
internal readonly record struct DependencyInfo(
    SystemType SystemType,
    Stage Stage,
    bool StartsCircularDependency
);