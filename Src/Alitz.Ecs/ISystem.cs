namespace Alitz.Ecs;
public interface ISystem
{
    void Update(ISystemContext context, long deltaMs);
}
