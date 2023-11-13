using System;



namespace Alitz.Ecs;
public class FactoryReturnTypeMismatchException : InstantiationException
{
    public FactoryReturnTypeMismatchException(Type systemType, Func<ISystem> factory, ISystem actualSystem)
    {
        SystemType = systemType;
        Factory = factory;
        ActualSystem = actualSystem;
    }

    public Type SystemType { get; }
    public Func<ISystem> Factory { get; }
    public ISystem ActualSystem { get; }

    public override string Message =>
        $"Expected system factory {Factory} (method: {Factory.Method})"
        + $" to return a system of type {SystemType}, got {ActualSystem.GetType()}.";
}