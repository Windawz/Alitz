using Alitz.Collections;

namespace Alitz;
public interface IEnvironment
{
    IPool<Entity> EntityPool { get; }

    IColumn<TComponent> Components<TComponent>() where TComponent : struct;
}
