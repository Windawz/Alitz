using System;
using System.Collections.Generic;
using System.Reflection;

using Xunit.Sdk;

namespace Alitz.UnitTests.Utilities;
public class RandomInt32DataAttribute : DataAttribute {
    private RandomInt32DataAttribute(
        int? seed = null,
        int setCount = 1,
        int minValue = Int32.MinValue,
        int maxValue = Int32.MaxValue
    ) {
        if (setCount < 0) {
            throw new ArgumentOutOfRangeException(nameof(setCount));
        }
        
        if (seed is not null) {
            _random = new Random(seed.Value);
        } else {
            _random = new Random();
        }
        _setCount = setCount;
        _minValue = minValue;
        _maxValue = maxValue;
    }
    
    private readonly Random _random;
    private readonly int _setCount;
    private readonly int _minValue;
    private readonly int _maxValue;
    
    /// <inheritdoc />
    public override IEnumerable<object[]> GetData(MethodInfo testMethod) {
        int parameterCount = testMethod.GetParameters().Length;
        for (int i = 0; i < _setCount; i++) {
            var arguments = new object[parameterCount];
            for (int j = 0; j < arguments.Length; j++) {
                arguments[j] = _random.Next(_minValue, _maxValue);
            }
            yield return arguments;
        }
    }
}
