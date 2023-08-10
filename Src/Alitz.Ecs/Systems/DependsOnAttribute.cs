using System;

namespace Alitz.Systems;
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class DependsOnAttribute : Attribute
{
    public DependsOnAttribute(Type systemType)
    {
        if (!systemType.IsAssignableTo(typeof(ISystem)))
        {
            throw new ArgumentException($"{systemType} is not a {nameof(ISystem)}", nameof(systemType));
        }
        SystemType = systemType;
    }

    public Type SystemType { get; }
}
