using System;

using Alitz.Common;

namespace Alitz.Engine;
public class DiscoveryException : AlitzException
{
    public DiscoveryException() { }

    public DiscoveryException(string? message) : base(message) { }

    public DiscoveryException(string? message, Exception? innerException) : base(message, innerException) { }
}
