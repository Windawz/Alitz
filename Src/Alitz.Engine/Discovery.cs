using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using Alitz.Systems;

namespace Alitz;
public static class Discovery
{
    public static IReadOnlyCollection<Type> DiscoverSystemTypes(string directoryName)
    {
        if (!Directory.Exists(directoryName))
        {
            throw new DiscoveryException($"Discovery directory {directoryName} does not exist");
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
            .Where(type => type.IsAssignableTo(typeof(ISystem)))
            .ToArray();
    }
}
