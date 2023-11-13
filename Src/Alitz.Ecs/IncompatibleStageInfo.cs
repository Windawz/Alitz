using System;

namespace Alitz.Ecs;
public readonly record struct IncompatibleStageInfo(Type Dependent, Stage DependentStage, Type Dependency, Stage DependencyStage);