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
internal class PluginCandidateCollection : IReadOnlyCollection<FileInfo>, IDisposable
{
    [MethodImpl(MethodImplOptions.NoInlining)]
    public PluginCandidateCollection(DirectoryInfo pluginDirectory)
    {
        _context = LoadCandidates(pluginDirectory);
        _assemblyFiles = GetValidCandidateFiles(_context);
    }

    private AssemblyLoadContext _context;
    private ICollection<FileInfo> _assemblyFiles;
    private bool _disposed = false;

    public int Count
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        get
        {
            ThrowIfDisposed();
            return _assemblyFiles.Count;
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public IEnumerator<FileInfo> GetEnumerator()
    {
        ThrowIfDisposed();
        return _assemblyFiles.GetEnumerator();
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

        _assemblyFiles.Clear();
        _assemblyFiles = null!;

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

    private static ICollection<FileInfo> GetValidCandidateFiles(AssemblyLoadContext candidateContext) =>
        candidateContext.Assemblies
            .Where(assembly => assembly.GetCustomAttribute<DiscoverableAttribute>() is not null)
            .Select(assembly => new FileInfo(assembly.Location))
            .ToList();

    private static AssemblyLoadContext LoadCandidates(DirectoryInfo pluginDirectory)
    {
        var context = new AssemblyLoadContext(name: "PluginCandidateContext", isCollectible: true);

        foreach (var file in EnumerateAssemblyFiles(pluginDirectory))
        {
            PluginAssembly.TryLoad(file, context);
        }

        return context;
    }

    private static IEnumerable<FileInfo> EnumerateAssemblyFiles(DirectoryInfo directory) =>
        EnumeratePotentialAssemblyFiles(directory)
            .Where(file => IsAssemblyFile(file));

    private static bool IsAssemblyFile(FileInfo file)
    {
        try
        {
            AssemblyName.GetAssemblyName(file.FullName);
        }
        catch (ArgumentException)
        {
            return false;
        }
        return true;
    }

    private static IEnumerable<FileInfo> EnumeratePotentialAssemblyFiles(DirectoryInfo directory) =>
        directory.EnumerateFiles("*.dll", SearchOption.TopDirectoryOnly);
}