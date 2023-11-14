namespace Alitz.EntityComponentSystem;
public interface ISystem
{
    void Update(ISystemContext context, long deltaMs);
}
