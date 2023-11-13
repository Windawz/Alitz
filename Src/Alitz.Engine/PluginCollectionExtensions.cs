using System;
using System.Collections.Generic;
using System.Linq;

using Alitz.Ecs;

namespace Alitz.Engine;
internal static class PluginCollectionExtensions
{
    public static IEnumerable<Type> EnumerateSystemTypes(this PluginCollection plugins) =>
        plugins.SelectMany(
            plugin => plugin.ExportedTypes
                .Where(type => SystemType.IsValid(type))
        );
}