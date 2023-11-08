using System;

namespace Alitz.Ecs.Systems;
public readonly record struct CircularDependencyInfo(Type Dependent, Type Dependency);