using System;

namespace Alitz;
internal class Application
{
    public Application()
    {
        var ecs = EntityComponentSystem.CreateBuilder()
            .AddSystems(Discovery.DiscoverSystemTypes(Environment.CurrentDirectory))
            .Build();

        var renderer = new Renderer(ecs);

        _gameLoop.IterationStarted += context => ecs.Update(context.DeltaMs);
        _gameLoop.IterationStarted += _ => renderer.Render();
    }

    private readonly GameLoop _gameLoop = new();

    public void Run() =>
        _gameLoop.Start();
}
