namespace Alitz.Ecs.Collections;
public class ComponentDictionary<TComponent> : SparseDictionary<Entity, TComponent>, IComponentDictionary<TComponent>
    where TComponent : struct
{
    public ComponentDictionary() : base(IndexExtractors.EntityIndexExtractor) { }
}
