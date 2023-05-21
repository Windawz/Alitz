namespace Alitz.Ecs; 
public interface IIndexProvider<T> where T : struct {
    int AsIndex(T value);
}
