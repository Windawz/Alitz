using System;
using System.IO;

using Alitz.Ecs;
using Alitz.Engine.Systems;

namespace Alitz.Engine;
internal class Application
{
    public Application()
    {
        _plugins = LoadPlugins(new DirectoryInfo(Environment.CurrentDirectory));

        var ecs = EntityComponentSystem.CreateBuilder()
            .AddSystems(_plugins.EnumerateSystemTypes())
            .AddSystem<InputSystem>()
            .AddSystem<RendererSystem>()
            .Build();

        _gameLoop.IterationStarted += context => ecs.Update(context.DeltaMs);
    }

    private readonly GameLoop _gameLoop = new();
    private PluginCollection _plugins;

    public void Run() =>
        _gameLoop.Start();

    private static PluginCollection LoadPlugins(DirectoryInfo directory)
    {
        using var candidates = new PluginCandidateCollection(directory);
        var plugins = new PluginCollection(candidates);
        AppDomain.CurrentDomain.ProcessExit += delegate
        {
            plugins.Dispose();
        };
        return plugins;
    }
}
