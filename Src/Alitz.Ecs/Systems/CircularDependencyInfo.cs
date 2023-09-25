namespace Alitz.Ecs.Systems;
internal readonly record struct CircularDependencyInfo(SystemType Dependent, SystemType Dependency);