using System;

namespace Alitz;
public static class Validators
{
    public static T ValidateType<T>(object value, string? exceptionParamName = null, string? exceptionMessage = null)
    {
        if (value is not T)
        {
            exceptionMessage ??= $"Parameter {(exceptionParamName ?? string.Empty) + ' '}has invalid type";
            throw new ArgumentException(exceptionMessage, exceptionParamName);
        }
        return (T)value;
    }
}
