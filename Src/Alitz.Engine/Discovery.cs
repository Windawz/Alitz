using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using Alitz.Ecs.Systems;

namespace Alitz.Engine;
public static class Discovery
{
    public static IEnumerable<SystemType> EnumerateSystemTypes(Assembly assembly) =>
        assembly.ExportedTypes
            .Where(type => SystemType.IsValid(type))
            .Cast<SystemType>();

    public static IEnumerable<Assembly> LoadAndEnumerateAssemblies(DirectoryInfo directory) =>
        EnumerateAssemblyFiles(directory)
            .Select(file =>
                {
                    try
                    {
                        return Assembly.LoadFile(file.FullName);
                    }
                    catch (Exception exception) when (exception is FileLoadException or BadImageFormatException or FileNotFoundException)
                    {
                        return null;
                    }
                })
            .Where(assembly => assembly is not null)
            .Cast<Assembly>();

    private static IEnumerable<FileInfo> EnumerateAssemblyFiles(DirectoryInfo directory) =>
        EnumeratePotentialAssemblyFileNames(directory)
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

    private static IEnumerable<FileInfo> EnumeratePotentialAssemblyFileNames(DirectoryInfo directory) =>
        directory.EnumerateFiles("*.dll", SearchOption.TopDirectoryOnly);
}
