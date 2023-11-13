using System;

namespace Alitz.Ecs;
public readonly record struct CircularDependencyInfo(Type Dependent, Type Dependency);