using System;

namespace Alitz.Ecs.Systems;
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class RunsAtStageAttribute : Attribute
{
    public RunsAtStageAttribute(int stageNumber) : this(new Stage(stageNumber)) { }

    public RunsAtStageAttribute(Stage stage)
    {
        Stage = stage;
    }

    public Stage Stage { get; }
}