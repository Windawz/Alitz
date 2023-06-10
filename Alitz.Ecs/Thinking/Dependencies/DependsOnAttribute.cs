using System;

namespace Alitz.Thinking.Dependencies;
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class DependsOnAttribute : Attribute
{
    public DependsOnAttribute(Type thinkerType)
    {
        if (!thinkerType.IsAssignableTo(typeof(Thinker)))
        {
            throw new ArgumentException($"{nameof(thinkerType)} is not a {nameof(Thinker)}", nameof(thinkerType));
        }
        ThinkerType = thinkerType;
    }

    public Type ThinkerType { get; }
}
