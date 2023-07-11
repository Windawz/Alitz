namespace Alitz.UnitTests;
public class MockIdFactoryTests : IdFactoryTests<MockIdFactory>
{
    public MockIdFactoryTests() : base(() => new MockIdFactory()) { }
}
