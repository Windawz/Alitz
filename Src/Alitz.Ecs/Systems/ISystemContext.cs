using Alitz.Common.Collections;
using Alitz.Ecs.Collections;

namespace Alitz.Ecs.Systems;
public interface ISystemContext
{
    IdPool EntityPool { get; }

    Column<TComponent> Components<TComponent>() where TComponent : struct;
}
