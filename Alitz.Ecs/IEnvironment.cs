using Alitz.Collections;

namespace Alitz;
public interface IEnvironment
{
    IEntityManager EntityManager { get; }

    IColumn<TComponent> Components<TComponent>() where TComponent : struct;
}
