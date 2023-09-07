using System;

namespace Alitz.Common;
public class AlitzException : Exception
{
    public AlitzException() { }

    public AlitzException(string? message) : base(message) { }

    public AlitzException(string? message, Exception? innerException) : base(message, innerException) { }
}
