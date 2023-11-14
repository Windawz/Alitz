using System;

using Alitz.Bridge.Components;
using Alitz.Bridge.Systems;
using Alitz.Common;
using Alitz.EntityComponentSystem;

namespace Alitz.Engine.Systems;
[HasDependency(typeof(RendererSystem))]
[RunsAtStage(ReservedStageNumbers.Engine)]
internal class InputSystem : ISystem
{
    public void Update(ISystemContext context, long deltaMs)
    {
        context.Components<InputComponent>().Clear();

        string input = Console.ReadLine()
            ?? throw new EngineException("Got null on Console.ReadLine()");
        
        Id inputEntity = context.EntityPool.Fetch();
        context.Components<InputComponent>()[inputEntity] = input;
    }
}