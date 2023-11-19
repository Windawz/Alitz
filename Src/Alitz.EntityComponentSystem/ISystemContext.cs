using Alitz.Common;

namespace Alitz.EntityComponentSystem;
public interface ISystemContext
{
    IEntitiesContext Entities { get; }

    IEntityContext? GetEntity(Id entity);
    IEntityContext NewEntity();
}