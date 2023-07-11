namespace Alitz.UnitTests;
public class MockIdFactory : IIdFactory<MockId>
{
    public int MinIndex =>
        MockId.MinIndex;

    public int MinVersion =>
        MockId.MinVersion;

    public int MaxIndex =>
        MockId.MaxIndex;

    public int MaxVersion =>
        MockId.MaxVersion;

    public MockId Create(int index, int version) =>
        new(index, version);
}
