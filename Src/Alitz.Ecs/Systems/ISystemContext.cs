using Alitz.Collections;

namespace Alitz.Systems;
public interface ISystemContext
{
    IdPool EntityPool { get; }

    IColumn<TComponent> Components<TComponent>() where TComponent : struct;
}
