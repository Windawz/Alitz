using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;

using Alitz.Bridge;

namespace Alitz.Engine;
internal class PluginCollection : IReadOnlyCollection<Assembly>, IDisposable
{
    private PluginCollection(IEnumerable<FileInfo> pluginFiles)
    {
        if (!pluginFiles.Any())
        {
            _context = null;
            _plugins = Array.Empty<Assembly>();
        }
        else
        {
            var plugins = new List<Assembly>();
            
            if (pluginFiles.TryGetNonEnumeratedCount(out int count))
            {
                plugins.EnsureCapacity(count);
            }

            _context = CreateLoadContext();
            
            foreach (var pluginFile in pluginFiles)
            {
                Assembly? plugin = LoadIntoContext(pluginFile, _context);
                if (plugin is not null)
                {
                    plugins.Add(plugin);
                }
            }

            _plugins = plugins;
        }
    }

    private AssemblyLoadContext? _context;
    private IReadOnlyCollection<Assembly> _plugins;
    private bool _disposed = false;

    public int Count
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        get
        {
            ThrowIfDisposed();
            return _plugins.Count;
        }
    }

    public static PluginCollection FromDirectory(DirectoryInfo directory)
    {
        var potentialPluginFiles = EnumeratePotentialPluginFiles(directory.EnumerateFiles());
        return FilterOut(
            new PluginCollection(potentialPluginFiles),
            plugin => plugin.GetCustomAttribute<PluginAttribute>() is not null
        );
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static PluginCollection FilterOut(PluginCollection source, Func<Assembly, bool> predicate) => 
        new PluginCollection(
            source.Where(predicate)
                .Select(plugin => new FileInfo(plugin.Location))
        );

    [MethodImpl(MethodImplOptions.NoInlining)]
    public IEnumerator<Assembly> GetEnumerator()
    {
        ThrowIfDisposed();
        return _plugins.GetEnumerator();
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    IEnumerator IEnumerable.GetEnumerator() =>
        GetEnumerator();

    [MethodImpl(MethodImplOptions.NoInlining)]
    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _plugins = null!;

        _context?.Unload();
        _context = null!;

        GC.Collect();

        _disposed = true;
    }

    private void ThrowIfDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(objectName: GetType().FullName);
        }
    }

    private AssemblyLoadContext CreateLoadContext() =>
        new(name: $"PluginContext#{GetHashCode()}", isCollectible: true);

    [MethodImpl(MethodImplOptions.NoInlining)]
    ~PluginCollection()
    {
        Dispose();
    }

    private static Assembly? LoadIntoContext(FileInfo pluginFile, AssemblyLoadContext context)
    {
        Assembly plugin;
        try
        {
            plugin = context.LoadFromAssemblyPath(pluginFile.FullName);
        }
        catch (Exception ex) when (ex is IOException or BadImageFormatException)
        {
            return null;
        }
        return plugin;
    }

    private static IEnumerable<FileInfo> EnumeratePotentialPluginFiles(IEnumerable<FileInfo> files) =>
        files.Where(file => file.Extension == ".dll")
            .Where(file => {
                    try
                    {
                        AssemblyName.GetAssemblyName(file.FullName);
                    }
                    catch (Exception ex) when (ex is ArgumentException or BadImageFormatException)
                    {
                        return false;
                    }
                    return true;
                }
            );
}