namespace Alitz.Components;
public readonly struct CurrentUserInputLineComponent
{
    public CurrentUserInputLineComponent(string? line)
    {
        Line = line;
    }

    public static CurrentUserInputLineComponent None =>
        new(null);

    public string? Line { get; }

    public static implicit operator CurrentUserInputLineComponent(string line) =>
        new(line);
}
