using System;

namespace Alitz.Ecs.Systems.Scheduling;
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class ForceStageAttribute : Attribute
{
    public ForceStageAttribute(int number)
    {
        Number = number;
    }

    public int Number { get; }
}