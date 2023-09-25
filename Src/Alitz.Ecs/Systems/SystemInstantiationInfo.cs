using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Alitz.Ecs.Systems;
internal class SystemInstantiationInfo
{
    public SystemInstantiationInfo(SystemType systemType, Func<ISystem>? systemFactory)
    {
        SystemFactory = systemFactory is not null
            ? ValidateFactory(systemFactory, systemType)
            : GetFactory(systemType);

        SystemType = systemType;
    }


    public Func<ISystem> SystemFactory { get; }
    public SystemType SystemType { get; }

    private static Func<ISystem> GetFactory(SystemType systemType)
    {
        var bindingFlags = BindingFlags.Public | BindingFlags.Instance;
        var parameterTypes = Array.Empty<Type>();

        var parameterlessConstructor = systemType.Type.GetConstructor(bindingFlags, parameterTypes);

        if (parameterlessConstructor is null)
        {
            throw new NoSuitableConstructorException(systemType.Type, bindingFlags, parameterTypes);
        }

        return Expression.Lambda<Func<ISystem>>(Expression.New(parameterlessConstructor)).Compile();
    }

    private static Func<ISystem> ValidateFactory(Func<ISystem> systemFactory, SystemType systemType)
    {
        var system = systemFactory();
        if (system.GetType() != systemType.Type)
        {
            throw new FactoryReturnTypeMismatchException(systemType.Type, systemFactory, system);
        }
        return systemFactory;
    }
}