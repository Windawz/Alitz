using System.Collections.Generic;
using System.Linq;

namespace Alitz.Ecs.Systems;
internal class SystemSchedule
{
    public SystemSchedule(IEnumerable<SystemFactory> factories)
    {
        var distinctFactories = factories.DistinctBy(factory => factory.SystemType).ToArray();
        _systems = Dependencies
            .GetSystemTypesOrderedByDependencies(distinctFactories.Select(factory => factory.SystemType))
            .Join(
                distinctFactories,
                orderedSystemType => orderedSystemType,
                factory => factory.SystemType,
                (_, factory) => factory)
            .Select(factory => factory.Create())
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
