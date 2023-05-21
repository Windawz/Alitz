namespace Alitz.Ecs; 
public struct Int32IndexProvider : IIndexProvider<int> {
    public int AsIndex(int value) =>
        value;
}
