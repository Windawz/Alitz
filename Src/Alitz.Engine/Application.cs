using System;
using System.IO;

using Alitz.Ecs;
using Alitz.Engine.Systems;

namespace Alitz.Engine;
internal class Application : IDisposable
{
    public Application()
    {
        _plugins = PluginCollection.FromDirectory(new DirectoryInfo(Environment.CurrentDirectory));

        var ecs = EntityComponentSystem.CreateBuilder()
            .AddSystems(_plugins.EnumerateSystemTypes())
            .AddSystem<InputSystem>()
            .AddSystem<RendererSystem>()
            .Build();

        _gameLoop = new GameLoop((_, deltaMs) =>
        {
            ecs.Update(deltaMs);
        });
    }

    private readonly GameLoop _gameLoop;
    private PluginCollection _plugins;
    private bool _disposed = false;

    public void Run() =>
        _gameLoop.Start();

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _plugins?.Dispose();
        _plugins = null!;

        _disposed = true;
    }
}
