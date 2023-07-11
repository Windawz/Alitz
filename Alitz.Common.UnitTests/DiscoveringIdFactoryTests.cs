namespace Alitz.UnitTests;
public class DiscoveringIdFactoryTests : IdFactoryTests<DiscoveringIdFactory<MockId>>
{
    public DiscoveringIdFactoryTests() : base(() => new DiscoveringIdFactory<MockId>()) { }
}
