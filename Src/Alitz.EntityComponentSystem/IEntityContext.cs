using Alitz.Common;

namespace Alitz.EntityComponentSystem;
public interface IEntityContext
{
    Id Entity { get; }

    ref TComponent Component<TComponent>() where TComponent : struct;
    void Destroy();
}