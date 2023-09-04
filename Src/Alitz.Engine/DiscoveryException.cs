using System;

namespace Alitz;
public class DiscoveryException : AlitzException
{
    public DiscoveryException() { }

    public DiscoveryException(string? message) : base(message) { }

    public DiscoveryException(string? message, Exception? innerException) : base(message, innerException) { }
}
