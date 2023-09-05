namespace Alitz.Systems;
public interface ISystem
{
    void Update(SystemContext context, long deltaMs);
}
