using System;

namespace Alitz;
public abstract class Singleton<TDerived> where TDerived : Singleton<TDerived> {
    public static TDerived Instance { get; } = CreateInstance();

    private static TDerived CreateInstance() =>
        (TDerived)Activator.CreateInstance(typeof(TDerived), true)!;
}
