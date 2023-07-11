using Alitz.Collections;

namespace Alitz;
public interface IColumnFactory
{
    IColumn<TComponent> Create<TComponent>() where TComponent : struct;
}
