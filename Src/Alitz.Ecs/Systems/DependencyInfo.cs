using System;

namespace Alitz.Ecs.Systems;
public readonly record struct DependencyInfo(
    Type SystemType,
    Stage Stage,
    bool StartsCircularDependency
);