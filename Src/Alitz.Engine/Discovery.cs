using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using Alitz.Ecs.Systems;

namespace Alitz.Engine;
public static class Discovery
{
    public static IEnumerable<Type> DiscoverSystemTypes(string directoryName)
    {
        if (!Directory.Exists(directoryName))
        {
            throw new DirectoryNotFoundException($"Could not find directory \"{directoryName}\"");
        }

        return Directory.EnumerateFiles(directoryName, "*.dll", SearchOption.TopDirectoryOnly)
            .Select(
                fileName =>
                {
                    Assembly? maybeAssembly;
                    try
                    {
                        maybeAssembly = Assembly.LoadFile(fileName);
                    }
                    catch (Exception exception) when (exception is FileLoadException or BadImageFormatException)
                    {
                        maybeAssembly = null;
                    }
                    return maybeAssembly;
                })
            .Where(maybeAssembly => maybeAssembly is not null)
            .Cast<Assembly>()
            .SelectMany(assembly => assembly.ExportedTypes)
            .Distinct()
            .Where(type => type.IsAssignableTo(typeof(ISystem)));
    }
}
