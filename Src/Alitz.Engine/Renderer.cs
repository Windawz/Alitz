using System;

using Alitz.Common;
using Alitz.Common.Components;
using Alitz.Ecs;
using Alitz.Ecs.Systems;

namespace Alitz.Engine;
public class Renderer
{
    public Renderer(EntityComponentSystem ecs)
    {
        _ecs = ecs;
    }

    private readonly EntityComponentSystem _ecs;

    public void Render() =>
        _ecs.ForEach(
            (Id _, ref GraphicComponent graphic, ref PositionComponent position) =>
            {
                Console.SetCursorPosition((int)position.X, (int)position.Y);
                Console.Write(graphic.CodePoint);
            });
}
