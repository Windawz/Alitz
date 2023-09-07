namespace Alitz.Engine;
internal interface IGameLoopIterationContext
{
    long DeltaMs { get; }
    bool IsRunning { get; set; }
}
