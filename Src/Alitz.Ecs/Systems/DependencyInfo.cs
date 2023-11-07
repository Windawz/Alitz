using System;

namespace Alitz.Ecs.Systems;
internal readonly record struct DependencyInfo(
    Type SystemType,
    Stage Stage,
    bool StartsCircularDependency
);