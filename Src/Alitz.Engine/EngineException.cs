using System;

using Alitz.Common;

namespace Alitz.Engine;
internal class EngineException : AlitzException
{
    public EngineException() { }

    public EngineException(string? message) : base(message) { }

    public EngineException(string? message, Exception? innerException) : base(message, innerException) { }
}