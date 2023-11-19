using System;

using Alitz.Bridge.Components;
using Alitz.Bridge.Systems;
using Alitz.EntityComponentSystem;

namespace Alitz.Engine.Systems;
[HasDependency(typeof(RendererSystem))]
[RunsAtStage(ReservedStageNumbers.Engine)]
internal class InputSystem : ISystem
{
    public void Update(ISystemContext context, long deltaMs)
    {
        context.Entities.ForEach((IEntityContext context, ref InputComponent component) =>
            context.Destroy()
        );

        string input = Console.ReadLine()
            ?? throw new EngineException("Got null on Console.ReadLine()");
        
        context.NewEntity().Component<InputComponent>() = input;
    }
}