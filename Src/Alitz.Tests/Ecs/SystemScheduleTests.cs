using Alitz.Ecs;

namespace Alitz.Tests.Ecs;
public class SystemScheduleTests
{
    public SystemScheduleTests()
    {
        var types = new Type[]
        {
            typeof(SystemA),
            typeof(SystemB),
            typeof(SystemC),
            typeof(SystemD),
        };

        _typesPerIndexInSchedule = new SystemSchedule(types)
            .Select((systemType, i) => (Type: systemType, Index: i))
            .ToDictionary(tuple => tuple.Type, tuple => tuple.Index);
    }

    private readonly IReadOnlyDictionary<Type, int> _typesPerIndexInSchedule;

    [Fact]
    public void NoDuplicateSystems()
    {
        Assert.Distinct(_typesPerIndexInSchedule.Keys);
    }

    [Fact]
    public void EnumerationOrderMatchesSystemDependencies()
    {
        Assert.Equal(0, Index<SystemA>());

        Assert.InRange(Index<SystemC>(), 1, 3);
        Assert.InRange(Index<SystemD>(), 1, 3);

        Assert.NotEqual(Index<SystemC>(), Index<SystemD>());

        Assert.InRange(
            Index<SystemB>(),
            Index<SystemC>() + 1,
            3
        );
    }

    private int Index<TSystem>() where TSystem : ISystem
    {
        if (_typesPerIndexInSchedule.TryGetValue(typeof(TSystem), out int index))
        {
            return index;
        }
        else
        {
            throw new ArgumentException($"No system of type {typeof(TSystem)}");
        }
    }

    public class SystemA : ISystem
    {
        public void Update(ISystemContext context, long deltaMs) { }
    }

    [HasDependency(typeof(SystemC))]
    public class SystemB : ISystem
    {
        public void Update(ISystemContext context, long deltaMs) { }
    }

    [HasDependency(typeof(SystemA))]
    public class SystemC : ISystem
    {
        public void Update(ISystemContext context, long deltaMs) { }
    }

    [HasDependency(typeof(SystemA))]
    public class SystemD : ISystem
    {
        public void Update(ISystemContext context, long deltaMs) { }
    }
}