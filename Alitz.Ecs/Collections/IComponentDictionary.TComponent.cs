namespace Alitz.Ecs.Collections;
public interface IComponentDictionary<TComponent> : ISparseDictionary<Entity, TComponent> where TComponent : struct { }
