using System;

namespace Alitz;
public class DiscoveringIdFactory<TId> : IIdFactory<TId> where TId : struct, IId<TId>
{
    private static readonly Id.Constructor<TId> DiscoveredConstructor;
    private static readonly int DiscoveredMinIndex;
    private static readonly int DiscoveredMinVersion;
    private static readonly int DiscoveredMaxIndex;
    private static readonly int DiscoveredMaxVersion;

    static DiscoveringIdFactory()
    {
        var constructor = Id.DiscoverConstructor<TId>();
        if (constructor is null)
        {
            throw new InvalidOperationException($"Failed to discover constructor for {typeof(TId)}");
        }
        var limits = Id.DiscoverLimits<TId>();
        if (limits is null)
        {
            throw new InvalidOperationException($"Failed to discover limits for {typeof(TId)}");
        }
        DiscoveredConstructor = constructor;
        (DiscoveredMinIndex, DiscoveredMinVersion, DiscoveredMaxIndex, DiscoveredMaxVersion) = limits.Value;
    }

    public int MinIndex =>
        DiscoveredMinIndex;

    public int MinVersion =>
        DiscoveredMinVersion;

    public int MaxIndex =>
        DiscoveredMaxIndex;

    public int MaxVersion =>
        DiscoveredMaxVersion;

    public TId Create(int index, int version) =>
        DiscoveredConstructor(index, version);
}
