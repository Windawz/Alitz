using System;

using Alitz.Ecs;
using Alitz.Engine.Systems;

namespace Alitz.Engine;
internal class Application
{
    public Application()
    {
        var ecs = EntityComponentSystem.CreateBuilder()
            .AddSystems(Discovery.DiscoverSystemTypes(Environment.CurrentDirectory))
            .AddSystem<RendererSystem>()
            .Build();

        _gameLoop.IterationStarted += context => ecs.Update(context.DeltaMs);
    }

    private readonly GameLoop _gameLoop = new();

    public void Run() =>
        _gameLoop.Start();
}
