namespace Alitz.Bridge.Components;
public readonly struct InputComponent
{
    public InputComponent()
    {
        Text = "";
    }

    public InputComponent(string text)
    {
        Text = text;
    }

    public string Text { get; }

    public static implicit operator InputComponent(string text) =>
        new(text);
}