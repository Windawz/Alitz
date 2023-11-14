using System;

namespace Alitz.EntityComponentSystem;
public readonly record struct DependencyInfo(
    Type SystemType,
    Stage Stage,
    bool StartsCircularDependency
);