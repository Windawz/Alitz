using Alitz.Common.Collections;


namespace Alitz.EntityComponentSystem;
public interface ISystemContext
{
    IdPool EntityPool { get; }

    Column<TComponent> Components<TComponent>() where TComponent : struct;
}
