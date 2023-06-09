namespace Alitz.Thinking.Dependencies;
public readonly record struct CircularDependency<T>(T Dependent, T Dependency) where T : notnull;
