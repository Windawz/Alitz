using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Alitz;
public class ReflectingIdFactory<TId> : IIdFactory<TId> where TId : struct, IId<TId>
{
    private static readonly Func<int, int, TId> FactoryMethod = MakeFactoryMethod();
    // This class has to exist, because it's important that the min and max values
    // of ids are part of the interface, and static abstract members are not allowed
    // in the given version of C#.
    // Writing a factory for every new Id type would become tedious, so
    // this one uses reflection and exploits the way static fields of generics work.
    // We only find the necessary values once per Id type.

    static ReflectingIdFactory()
    {
        (FoundMinIndex, FoundMinVersion, FoundMaxIndex, FoundMaxVersion) = FindLimits();
    }

    public int MinIndex =>
        FoundMinIndex;

    public int MinVersion =>
        FoundMinVersion;

    public int MaxIndex =>
        FoundMaxIndex;

    public int MaxVersion =>
        FoundMaxVersion;

    public TId Create(int index, int version) =>
        FactoryMethod(index, version);

    private static (int minIndex, int minVersion, int maxIndex, int maxVersion) FindLimits()
    {
        Dictionary<string, int?> limitsPerName = new()
        {
            { nameof(IIdFactory<TId>.MinIndex), null },
            { nameof(IIdFactory<TId>.MinVersion), null },
            { nameof(IIdFactory<TId>.MaxIndex), null },
            { nameof(IIdFactory<TId>.MaxVersion), null },
        };

        AssignLimits(limitsPerName, KeepOnlyRelevant(GetPublicStaticFields(), limitsPerName.Keys));
        if (!HasAllBeenAssigned(limitsPerName))
        {
            AssignLimits(limitsPerName, KeepOnlyRelevant(GetPublicStaticProperties(), limitsPerName.Keys));
        }
        if (!HasAllBeenAssigned(limitsPerName))
        {
            string missingLimits = string.Join(", ", limitsPerName.Where(kv => kv.Value is null).Select(kv => kv.Key));
            throw new InvalidOperationException($"Failed to find limits {missingLimits} of id type {typeof(TId)}");
        }
        return (minIndex: (int)limitsPerName[nameof(IIdFactory<TId>.MinIndex)]!,
            minVersion: (int)limitsPerName[nameof(IIdFactory<TId>.MinVersion)]!,
            maxIndex: (int)limitsPerName[nameof(IIdFactory<TId>.MaxIndex)]!,
            maxVersion: (int)limitsPerName[nameof(IIdFactory<TId>.MaxVersion)]!);

        static bool HasAllBeenAssigned(IReadOnlyDictionary<string, int?> limitsPerName) =>
            limitsPerName.Values.All(value => value is not null);

        static FieldInfo[] GetPublicStaticFields() =>
            typeof(TId).GetFields(BindingFlags.Public | BindingFlags.Static);

        static PropertyInfo[] GetPublicStaticProperties() =>
            typeof(TId).GetProperties(BindingFlags.Public | BindingFlags.Static);

        static IEnumerable<MemberInfo> KeepOnlyRelevant(IEnumerable<MemberInfo> members, IEnumerable<string> limitNames) =>
            members.Where(
                member => limitNames.Contains(member.Name)
                    && (member is FieldInfo field && field.FieldType == typeof(int)
                        || member is PropertyInfo property && property.PropertyType == typeof(int)));

        static void AssignLimits(IDictionary<string, int?> limitsPerName, IEnumerable<MemberInfo> limitFieldsOrProperties)
        {
            foreach (var member in limitFieldsOrProperties)
            {
                limitsPerName[member.Name] = ExtractValue(member);
            }
        }

        static int ExtractValue(MemberInfo fieldOrProperty)
        {
            switch (fieldOrProperty)
            {
                case PropertyInfo property:
                    return (int)property.GetValue(null)!;
                case FieldInfo field:
                    return (int)field.GetValue(null)!;
                default:
                    throw new ArgumentException("Expected a field or property", nameof(fieldOrProperty));
            }
        }
    }

    private static Func<int, int, TId> MakeFactoryMethod()
    {
        var constructor = typeof(TId).GetConstructor(
            BindingFlags.Public | BindingFlags.Instance,
            new[] { typeof(int), typeof(int), });
        if (constructor is null)
        {
            throw new InvalidOperationException($"No suitable constructor found on {typeof(TId)}");
        }
        ParameterExpression[] parameters =
        {
            Expression.Parameter(typeof(int), "index"), Expression.Parameter(typeof(int), "version"),
        };
        return Expression.Lambda<Func<int, int, TId>>(Expression.New(constructor, parameters), parameters).Compile();
    }

    // ReSharper disable StaticMemberInGenericType
    private static readonly int FoundMinIndex;
    private static readonly int FoundMinVersion;
    private static readonly int FoundMaxIndex;
    private static readonly int FoundMaxVersion;
    // ReSharper restore StaticMemberInGenericType
}
