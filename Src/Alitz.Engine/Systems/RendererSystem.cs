using System;

using Alitz.Bridge.Components;
using Alitz.Bridge.Systems;
using Alitz.EntityComponentSystem;

namespace Alitz.Engine.Systems;
[RunsAtStage(ReservedStageNumbers.Engine)]
internal class RendererSystem : ISystem
{
    public void Update(ISystemContext context, long deltaMs)
    {
        Console.Clear();
        context.Entities.ForEach((IEntityContext context, ref GraphicComponent graphic, ref PositionComponent position) =>
        {
            Console.SetCursorPosition((int)position.X, (int)position.Y);
            Console.Write(graphic.CodePoint);
        });
    }
}