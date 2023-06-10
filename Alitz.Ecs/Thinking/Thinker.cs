namespace Alitz.Thinking;
public abstract class Thinker
{
    protected Thinker(Environment environment)
    {
        Environment = environment;
    }

    protected Environment Environment { get; }

    public abstract void Think();
}
