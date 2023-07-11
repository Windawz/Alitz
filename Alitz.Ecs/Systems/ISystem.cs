namespace Alitz.Systems;
public interface ISystem
{
    void Update(ISystemContext context, double delta);
}
