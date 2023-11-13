using System;
using System.Collections.Generic;
using System.Linq;

namespace Alitz.Ecs;
public class FactoryResolutionException : InstantiationException
{
    public FactoryResolutionException(Type systemType, IReadOnlyList<Type> parameterTypes)
    {
        SystemType = systemType;
        ParameterTypes = parameterTypes.ToArray();
    }

    public Type SystemType { get; }
    public IReadOnlyList<Type> ParameterTypes { get; }

    public override string Message =>
        $"Failed to resolve factory with parameter types {{"
        + string.Join(", ", ParameterTypes)
        + "}} for system "
        + SystemType.ToString();
}