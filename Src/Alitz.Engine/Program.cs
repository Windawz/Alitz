namespace Alitz.Engine;
public static class Program
{
    public static void Main(string[] args)
    {
        using var application = new Application();
        application.Run();
    }
}
