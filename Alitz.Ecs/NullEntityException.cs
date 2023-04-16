using System;

namespace Alitz.Ecs;
public class NullEntityException : ArgumentException {
    public NullEntityException(
        string? message = null,
        string? paramName = null,
        Exception? innerException = null
    ) : base(message, paramName, innerException) { }
}
