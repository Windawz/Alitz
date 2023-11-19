namespace Alitz.EntityComponentSystem;
public interface ITable
{
    Column<TComponent> Column<TComponent>() where TComponent : struct;
}