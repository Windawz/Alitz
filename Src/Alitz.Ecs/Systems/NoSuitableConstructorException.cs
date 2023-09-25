using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Alitz.Ecs.Systems;
public class NoSuitableConstructorException : InstantiationException
{
    public NoSuitableConstructorException(Type systemType, BindingFlags bindingFlags, IReadOnlyList<Type> parameterTypes)
    {
        SystemType = systemType;
        BindingFlags = bindingFlags;
        ParameterTypes = parameterTypes.ToArray();
    }

    public Type SystemType { get; }
    public BindingFlags BindingFlags { get; }
    public IReadOnlyList<Type> ParameterTypes { get; }

    public override string Message =>
        $"Failed to find constructor with binding flags "
        + Convert.ToString((int)BindingFlags, 2).PadLeft(8, '0')
        + " and parameter types {{"
        + string.Join(", ", ParameterTypes)
        + "}} on system of type "
        + SystemType.ToString();
}