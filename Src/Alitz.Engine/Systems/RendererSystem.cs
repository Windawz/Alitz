using System;

using Alitz.Common;
using Alitz.Common.Components;
using Alitz.Ecs.Systems;
using Alitz.Ecs.Systems.Scheduling;

namespace Alitz.Engine.Systems;
[ForceStage(-1)]
internal class RendererSystem : ISystem
{
    public void Update(ISystemContext context, long deltaMs) => 
        context.ForEach((Id entity, ref GraphicComponent graphic, ref PositionComponent position) =>
        {
            Console.SetCursorPosition((int)position.X, (int)position.Y);
            Console.Write(graphic.CodePoint);
        });
}