namespace Alitz;
internal class Application
{
    private readonly MainLoop _mainLoop = new((_, _) => { }, (_, _) => { }, _ => { });

    public void Run() =>
        _mainLoop.Start();
}
