using System;

namespace Alitz;
internal class Application
{
    public Application()
    {
        var ecs = EntityComponentSystem.CreateBuilder()
            .AddSystems(Discovery.DiscoverSystemTypes(Environment.CurrentDirectory))
            .Build();

        _mainLoop.UpdateStarted += deltaMs => ecs.Update(deltaMs);
    }

    private readonly MainLoop _mainLoop = new();

    public void Run() =>
        _mainLoop.Start();
}
