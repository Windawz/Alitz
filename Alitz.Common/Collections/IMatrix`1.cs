namespace Alitz.Collections;
public interface IMatrix<T>
{
    int Count { get; }
    int Width { get; }
    int Height { get; }

    ref T this[int x, int y] { get; }
    ref T this[int index] { get; }

    void Fill(T value)
    {
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                this[j, i] = value;
            }
        }
    }

    void Resize(int width, int height);
}
