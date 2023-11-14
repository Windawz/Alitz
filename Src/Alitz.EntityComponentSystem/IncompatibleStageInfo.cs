using System;

namespace Alitz.EntityComponentSystem;
public readonly record struct IncompatibleStageInfo(Type Dependent, Stage DependentStage, Type Dependency, Stage DependencyStage);