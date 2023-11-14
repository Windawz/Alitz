using System;

namespace Alitz.EntityComponentSystem;
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class RunsAtStageAttribute : Attribute
{
    public RunsAtStageAttribute(int stageNumber)
    {
        Stage = new(stageNumber);
    }

    public Stage Stage { get; }
}