using System;

namespace Alitz.Systems;
internal class SystemFactory
{
    private SystemFactory(Func<ISystem> factoryMethod, Type systemType)
    {
        _factoryMethod = factoryMethod;
        SystemType = systemType;
    }

    private readonly Func<ISystem> _factoryMethod;

    public Type SystemType { get; }

    public static SystemFactory Create<TSystem>(Func<TSystem> factoryMethod) where TSystem : class, ISystem =>
        new(factoryMethod, typeof(TSystem));

    public ISystem Create() =>
        _factoryMethod();
}
