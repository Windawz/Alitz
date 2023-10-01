using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Alitz.Ecs.Systems;

namespace Alitz.Engine;
internal static class PluginCollectionExtensions
{
    public static IEnumerable<SystemType> EnumerateSystemTypes(this PluginCollection plugins) =>
        plugins.SelectMany(plugin => EnumerateSystemTypesInAssembly(plugin));

    private static IEnumerable<SystemType> EnumerateSystemTypesInAssembly(Assembly assembly) =>
        assembly.ExportedTypes
            .Where(type => SystemType.IsValid(type))
            .Cast<SystemType>();
}