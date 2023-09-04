using System;
using System.Diagnostics;

namespace Alitz;
internal class MainLoop
{
    public MainLoop(Action<ConsoleKeyInfo?> inputAction, Action<long> updateAction, Action renderAction)
    {
        _inputAction = inputAction;
        _updateAction = updateAction;
        _renderAction = renderAction;
        _stopwatch = new Stopwatch();
    }

    private readonly Action<ConsoleKeyInfo?> _inputAction;
    private readonly Action _renderAction;
    private readonly Stopwatch _stopwatch;
    private readonly Action<long> _updateAction;
    private long _previousDeltaMs;

    public void Start()
    {
        while (true)
        {
            _stopwatch.Restart();
            _inputAction(GetInputIfAny());
            UpdateCompensatingForDeltaSpikes(_updateAction, _previousDeltaMs);
            _renderAction();
            _stopwatch.Stop();
            _previousDeltaMs = _stopwatch.ElapsedMilliseconds;
        }
    }

    private static void UpdateCompensatingForDeltaSpikes(Action<long> updateAction, long deltaMs)
    {
        const long maxStepMs = 20;
        while (deltaMs > 0)
        {
            long stepMs = Math.Min(deltaMs, maxStepMs);
            updateAction(stepMs);
            deltaMs -= stepMs;
        }
    }

    private static ConsoleKeyInfo? GetInputIfAny()
    {
        if (Console.KeyAvailable)
        {
            return Console.ReadKey(true);
        }
        return null;
    }
}
