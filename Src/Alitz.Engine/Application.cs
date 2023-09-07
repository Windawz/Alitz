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

        var inputHandler = new UserInputHandler(ecs);

        _mainLoop.UpdateStarted += deltaMs => ecs.Update(deltaMs);
        _mainLoop.RenderStarted += () => renderer.Render();
        _mainLoop.InputChecked += maybeKeyInfo => inputHandler.Handle(maybeKeyInfo);
    }

    private readonly MainLoop _mainLoop = new();

    public void Run() =>
        _mainLoop.Start();
}
