using System;
using System.IO;
using System.Runtime.Loader;

namespace Alitz.Engine;
internal static class PluginAssembly
{
    public static bool TryLoad(FileInfo assemblyFile, AssemblyLoadContext context) =>
        TryLoad(assemblyFile, context, out _);

    public static bool TryLoad(FileInfo assemblyFile, AssemblyLoadContext context, out Exception? exception)
    {
        exception = null;
        try
        {
            context.LoadFromAssemblyPath(assemblyFile.FullName);
        }
        catch (Exception ex) when (ex is IOException or BadImageFormatException)
        {
            exception = ex;
            return false;
        }
        return true;
    }
}