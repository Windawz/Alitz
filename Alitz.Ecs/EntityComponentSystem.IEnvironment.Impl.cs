using Alitz.Collections;

namespace Alitz;
public partial class EntityComponentSystem
{
    IEntityManager IEnvironment.EntityManager =>
        _entityManager;

    IColumn<TComponent> IEnvironment.Components<TComponent>() where TComponent : struct
    {
        var componentType = typeof(TComponent);
        if (!_columnTable.ContainsKey(componentType))
        {
            var column = new EntityAssociatedColumn<TComponent>(new SparseColumn<TComponent>(), _entityManager);
            _columnTable.Add(componentType, column);
        }
        return (IColumn<TComponent>)_columnTable[componentType];
    }
}
