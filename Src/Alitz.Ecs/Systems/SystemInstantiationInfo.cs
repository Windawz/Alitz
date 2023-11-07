using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Alitz.Ecs.Systems;
internal class SystemInstantiationInfo
{
    public SystemInstantiationInfo(Type systemType, Func<ISystem>? systemFactory)
    {
        Systems.SystemType.ThrowIfNotValid(systemType, paramName: nameof(systemType));
    
        SystemFactory = systemFactory is not null
            ? ValidateFactory(systemFactory, systemType)
            : GetFactory(systemType);

        SystemType = systemType;
    }


    public Func<ISystem> SystemFactory { get; }
    public Type SystemType { get; }

    private static Func<ISystem> GetFactory(Type systemType)
    {
        var bindingFlags = BindingFlags.Public | BindingFlags.Instance;
        var parameterTypes = Array.Empty<Type>();

        var parameterlessConstructor = systemType.GetConstructor(bindingFlags, parameterTypes);

        if (parameterlessConstructor is null)
        {
            throw new NoSuitableConstructorException(systemType, bindingFlags, parameterTypes);
        }

        return Expression.Lambda<Func<ISystem>>(Expression.New(parameterlessConstructor)).Compile();
    }

    private static Func<ISystem> ValidateFactory(Func<ISystem> systemFactory, Type systemType)
    {
        var system = systemFactory();
        if (system.GetType() != systemType)
        {
            throw new FactoryReturnTypeMismatchException(systemType, systemFactory, system);
        }
        return systemFactory;
    }
}