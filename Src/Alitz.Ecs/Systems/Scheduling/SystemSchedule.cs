using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Alitz.Ecs.Systems.Scheduling;
internal class SystemSchedule
{
    public SystemSchedule(IEnumerable<SystemFactory> factories)
    {
        const int defaultStageNumber = 0;
        
        var distinctFactories = factories.DistinctBy(factory => factory.SystemType).ToArray();
        _systems = Dependencies
            .GetSystemTypesOrderedByDependencies(distinctFactories.Select(factory => factory.SystemType))
            .GroupBy(systemType => systemType.GetCustomAttribute<ForceStageAttribute>()?.Number ?? defaultStageNumber)
            .OrderBy(grouping => grouping.Key)
            .SelectMany(grouping =>
                {
                    int stageNumberOfThisGrouping = grouping.Key;
                    return grouping.Select(systemType =>
                        {
                            var dependenciesAndTheirStageNumbers = systemType
                                .GetCustomAttributes<DependsOnAttribute>()
                                .Select(attribute => 
                                    (
                                        dependencyType: attribute.SystemType,
                                        dependencyStageNumber: 
                                            attribute.SystemType.GetCustomAttribute<ForceStageAttribute>()?.Number
                                                ?? defaultStageNumber
                                    )
                                ).ToArray();

                            bool anyDependenciesThatRunAtLaterStages = dependenciesAndTheirStageNumbers
                                .Any(tuple => tuple.dependencyStageNumber > stageNumberOfThisGrouping);
                            
                            if (anyDependenciesThatRunAtLaterStages)
                            {
                                throw new CircularDependencyException(
                                    dependentSystemType: systemType,
                                    dependencySystemType: dependenciesAndTheirStageNumbers.First().dependencyType);
                            }

                            return systemType;
                        });
                })
            .Join(
                inner: distinctFactories,
                outerKeySelector: orderedSystemType => orderedSystemType,
                innerKeySelector: factory => factory.SystemType,
                resultSelector: (_, factory) => factory.Create())
            .ToArray();
    }

    private readonly ISystem[] _systems;

    public void Update(ISystemContext context, long elapsedMilliseconds)
    {
        for (int i = 0; i < _systems.Length; i++)
        {
            _systems[i].Update(context, elapsedMilliseconds);
        }
    }
}
