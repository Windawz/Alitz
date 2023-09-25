namespace Alitz.Ecs.Systems;
internal readonly record struct IncompatibleStageInfo(SystemType Dependent, Stage DependentStage, SystemType Dependency, Stage DependencyStage);