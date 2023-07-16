using System;

using Alitz;

public class EcsException : AlitzException
{
    public EcsException() { }

    public EcsException(string message) : base(message) { }

    public EcsException(string message, Exception inner) : base(message, inner) { }
}
