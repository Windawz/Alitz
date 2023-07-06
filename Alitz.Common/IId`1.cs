using System;

namespace Alitz;
public interface IId<TId> : IEquatable<TId> where TId : struct, IId<TId>
{
    public int Index { get; }
    public int Version { get; }
}
