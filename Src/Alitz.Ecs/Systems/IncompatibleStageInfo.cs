using System;

namespace Alitz.Ecs.Systems;
internal readonly record struct IncompatibleStageInfo(Type Dependent, Stage DependentStage, Type Dependency, Stage DependencyStage);