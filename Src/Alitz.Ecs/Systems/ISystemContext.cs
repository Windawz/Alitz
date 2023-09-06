using Alitz.Collections;

namespace Alitz.Systems;
public interface ISystemContext
{
    IdPool EntityPool { get; }

    Column<TComponent> Components<TComponent>() where TComponent : struct;
}
