using System;

namespace Alitz.UnitTests;
public abstract class IdFactoryTests<TIdFactory> where TIdFactory : IIdFactory<MockId>
{
    public IdFactoryTests(Func<TIdFactory> idFactoryFactory)
    {
        _factory = idFactoryFactory();
    }

    private readonly TIdFactory _factory;

    [Fact]
    public void LimitPropertiesDoMatchIdTypeConstants()
    {
        Assert.Equal(MockId.MinIndex, _factory.MinIndex);
        Assert.Equal(MockId.MinVersion, _factory.MinVersion);
        Assert.Equal(MockId.MaxIndex, _factory.MaxIndex);
        Assert.Equal(MockId.MaxVersion, _factory.MaxVersion);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(42, 63)]
    public void Create_IndexAndVersionMatchThoseOfId(int index, int version)
    {
        var manualId = new MockId(index, version);
        var factoryId = _factory.Create(index, version);
        Assert.Equal(manualId.Index, factoryId.Index);
        Assert.Equal(manualId.Version, factoryId.Version);
    }

    [Fact]
    public void Create_OutOfRangeValuesCauseException()
    {
        (int index, int version)[] outOfRangeValues =
        {
            (_factory.MinIndex - 1, 0),
            (_factory.MaxIndex + 1, 0),
            (0, _factory.MinVersion - 1),
            (0, _factory.MaxVersion + 1),
            (_factory.MinIndex - 1, _factory.MinVersion - 1),
            (_factory.MaxIndex + 1, _factory.MinVersion - 1),
            (_factory.MinIndex - 1, _factory.MaxVersion + 1),
            (_factory.MaxIndex + 1, _factory.MaxVersion + 1),
        };
        foreach ((int index, int version) in outOfRangeValues)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _factory.Create(index, version));
        }
    }
}
