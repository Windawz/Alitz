using System;

namespace Alitz.Ecs.Systems;
internal class SystemFactory
{
    public SystemFactory(Type systemType, Func<ISystem> factory, bool prevalidateFactoryType = false)
    {
        if (!systemType.IsAssignableTo(typeof(ISystem)))
        {
            throw new ArgumentException(
                $"Provided system type is not assignable to {nameof(ISystem)}",
                nameof(systemType));
        }
        SystemType = systemType;
        _factory = factory;
        _prevalidateFactoryType = prevalidateFactoryType;
        if (_prevalidateFactoryType)
        {
            var dummyInstance = _factory();
            if (!SystemType.IsInstanceOfType(dummyInstance))
            {
                throw new ArgumentException(
                    MakeExceptionMessageAboutFactoryTypeMismatch(SystemType, dummyInstance.GetType()),
                    nameof(factory));
            }
        }
    }

    private readonly Func<ISystem> _factory;
    private readonly bool _prevalidateFactoryType;

    public Type SystemType { get; }

    public ISystem Create()
    {
        var instance = _factory();
        if (!_prevalidateFactoryType && !SystemType.IsInstanceOfType(instance))
        {
            throw new InvalidOperationException(
                MakeExceptionMessageAboutFactoryTypeMismatch(SystemType, instance.GetType()));
        }
        return instance;
    }

    private static string MakeExceptionMessageAboutFactoryTypeMismatch(Type systemType, Type instanceType) =>
        "Provided factory for system of type " + $"{systemType} produces an instance of a different type ({instanceType})";
}
