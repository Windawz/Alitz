using Alitz.Common.Collections;


namespace Alitz.Ecs;
public interface ISystemContext
{
    IdPool EntityPool { get; }

    Column<TComponent> Components<TComponent>() where TComponent : struct;
}
