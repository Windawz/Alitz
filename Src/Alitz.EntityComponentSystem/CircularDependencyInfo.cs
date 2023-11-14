using System;

namespace Alitz.EntityComponentSystem;
public readonly record struct CircularDependencyInfo(Type Dependent, Type Dependency);