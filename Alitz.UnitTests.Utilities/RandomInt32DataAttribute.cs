using System.Reflection;

using Xunit.Sdk;

namespace Alitz.UnitTests;
public class RandomInt32DataAttribute : DataAttribute
{
    public RandomInt32DataAttribute()
    {
        Seed = null;
        SetCount = 1;
        MinValue = int.MinValue;
        MaxValue = int.MaxValue;
    }

    private readonly int _setCount;

    public int? Seed { get; init; }
    public int SetCount
    {
        get => _setCount;
        init
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }
            _setCount = value;
        }
    }
    public int MinValue { get; init; }
    public int MaxValue { get; init; }

    /// <inheritdoc />
    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        var random = Seed is null ? new Random() : new Random(Seed.Value);
        int parameterCount = testMethod.GetParameters().Length;
        for (int i = 0; i < SetCount; i++)
        {
            object[] arguments = new object[parameterCount];
            for (int j = 0; j < arguments.Length; j++)
            {
                arguments[j] = random.Next(MinValue, MaxValue);
            }
            yield return arguments;
        }
    }
}
