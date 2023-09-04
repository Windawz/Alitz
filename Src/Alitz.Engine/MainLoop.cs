using System;
using System.Diagnostics;

namespace Alitz;
internal class MainLoop
{
    public MainLoop(MainLoopInputAction inputAction, MainLoopUpdateAction updateAction, MainLoopRenderAction renderAction)
    {
        _inputAction = inputAction;
        _updateAction = deltaMs => updateAction(deltaMs, () => _isRunning = false);
        _renderAction = renderAction;
        _stopwatch = new Stopwatch();
    }

    private readonly MainLoopInputAction _inputAction;
    private readonly MainLoopRenderAction _renderAction;
    private readonly Stopwatch _stopwatch;
    private readonly Action<long> _updateAction;
    private bool _isRunning;
    private long _previousDeltaMs;

    public void Start()
    {
        _isRunning = true;
        while (_isRunning)
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
