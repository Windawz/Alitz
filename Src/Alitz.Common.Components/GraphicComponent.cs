namespace Alitz.Components;
public readonly struct GraphicComponent
{
    public GraphicComponent(string codePoint)
    {
        if (codePoint.Length == 0 || codePoint.Length > 2)
        {
            throw new ArgumentException(
                "Code point length must be greater than zero and no greater than two",
                nameof(codePoint));
        }
        CodePoint = codePoint;
    }

    public string CodePoint { get; }

    public static implicit operator GraphicComponent(string codePoint) =>
        new(codePoint);

    public static implicit operator GraphicComponent(char character) =>
        new(character.ToString());
}
