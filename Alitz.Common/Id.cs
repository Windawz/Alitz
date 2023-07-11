using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Alitz;
public static class Id
{
    public delegate TId Constructor<TId>(int index, int version) where TId : struct, IId<TId>;

    private static readonly string MinIndexLimitMemberName = "MinIndex";
    private static readonly string MinVersionLimitMemberName = "MinVersion";
    private static readonly string MaxIndexLimitMemberName = "MaxIndex";
    private static readonly string MaxVersionLimitMemberName = "MaxVersion";
    private static readonly Type IndexAndVersionAccessorReturnType = typeof(int);

    public static Constructor<TId>? DiscoverConstructor<TId>() where TId : struct, IId<TId>
    {
        var constructor = typeof(TId).GetConstructor(
            BindingFlags.Public | BindingFlags.Instance,
            new[] { IndexAndVersionAccessorReturnType, IndexAndVersionAccessorReturnType, });

        if (constructor is null)
        {
            return null;
        }

        var indexParameter = Expression.Parameter(IndexAndVersionAccessorReturnType, "index");
        var versionParameter = Expression.Parameter(IndexAndVersionAccessorReturnType, "version");

        return Expression.Lambda<Constructor<TId>>(
                Expression.New(constructor, indexParameter, versionParameter),
                indexParameter,
                versionParameter)
            .Compile();
    }

    public static (int minIndex, int minVersion, int maxIndex, int maxVersion)? DiscoverLimits<TId>()
        where TId : struct, IId<TId>
    {
        int? minIndex = DiscoverLimit<TId>(MinIndexLimitMemberName);
        int? minVersion = DiscoverLimit<TId>(MinVersionLimitMemberName);
        int? maxIndex = DiscoverLimit<TId>(MaxIndexLimitMemberName);
        int? maxVersion = DiscoverLimit<TId>(MaxVersionLimitMemberName);

        if (minIndex is not null && minVersion is not null && maxIndex is not null && maxVersion is not null)
        {
            return (minIndex.Value, minVersion.Value, maxIndex.Value, maxVersion.Value);
        }
        return null;
    }

    private static int? DiscoverLimit<TId>(string limitName) where TId : struct, IId<TId>
    {
        var members = typeof(TId).GetMembers(BindingFlags.Public | BindingFlags.Static);
        var matchedMember = members.Where(member => member.Name == limitName).SingleOrDefault();

        int? limit = null;

        switch (matchedMember)
        {
            case FieldInfo field:
                if (field.FieldType == IndexAndVersionAccessorReturnType)
                {
                    limit = (int)field.GetValue(null)!;
                }
                break;
            case PropertyInfo property:
                if (property.PropertyType == IndexAndVersionAccessorReturnType)
                {
                    limit = (int)property.GetValue(null)!;
                }
                break;
        }

        return limit;
    }
}
