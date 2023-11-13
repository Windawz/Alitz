using System;

namespace Alitz.Ecs;
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class HasDependencyAttribute : Attribute
{
    public HasDependencyAttribute(Type type)
    {
        SystemType = type;
    }

    public Type SystemType { get; }
}
