namespace Alitz.Ecs.Collections;
public interface IComponentDictionary<TComponent> : IComponentDictionary, ISparseDictionary<Entity, TComponent>
    where TComponent : struct { }
