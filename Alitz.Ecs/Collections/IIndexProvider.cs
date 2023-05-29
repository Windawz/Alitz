namespace Alitz.Ecs.Collections;
public interface IIndexProvider<T> where T : struct {
    int AsIndex(T value);
}
