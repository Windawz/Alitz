using Alitz.Collections;

namespace Alitz.Systems;
public interface ISystemContext
{
    IPool<Id> EntityPool { get; }

    IColumn<TComponent> Components<TComponent>() where TComponent : struct;
}
