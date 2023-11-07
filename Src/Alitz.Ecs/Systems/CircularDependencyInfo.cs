using System;

namespace Alitz.Ecs.Systems;
internal readonly record struct CircularDependencyInfo(Type Dependent, Type Dependency);