using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;

namespace Alitz.Engine;
internal class PluginCollection : IReadOnlyCollection<Assembly>, IDisposable
{
    [MethodImpl(MethodImplOptions.NoInlining)]
    public PluginCollection(PluginCandidateCollection candidates)
    {
        _context = LoadPluginFiles(candidates);
        _assemblies = _context.Assemblies.ToList();
    }

    private AssemblyLoadContext _context;
    private ICollection<Assembly> _assemblies;
    private bool _disposed = false;

    public int Count
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        get
        {
            ThrowIfDisposed();
            return _assemblies.Count;
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public IEnumerator<Assembly> GetEnumerator()
    {
        ThrowIfDisposed();
        return _assemblies.GetEnumerator();
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

        _assemblies.Clear();
        _assemblies = null!;

        _context.Unload();
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

    private static AssemblyLoadContext LoadPluginFiles(IEnumerable<FileInfo> assemblyFiles)
    {
        var context = new AssemblyLoadContext(name: "PluginContext", isCollectible: true);
        foreach (var file in assemblyFiles)
        {
            PluginAssembly.TryLoad(file, context);
        }
        return context;
    }
}