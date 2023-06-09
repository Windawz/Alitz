namespace Alitz.Thinking;
public abstract class Thinker
{
    protected Thinker(Environment environment)
    {
        Environment = environment;
    }

    protected Environment Environment { get; }

    public abstract void Think();

    internal void Configure(Configuration.Builder builder) =>
        OnConfigureDependencies(builder);

    protected virtual void OnConfigureDependencies(IDependencyConfigurationBuilder builder) { }
}
