using System;

namespace Alitz.Ecs;
public readonly record struct DependencyInfo(
    Type SystemType,
    Stage Stage,
    bool StartsCircularDependency
);