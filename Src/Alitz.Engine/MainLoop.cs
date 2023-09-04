using System;
using System.Diagnostics;

namespace Alitz;
internal class MainLoop
{
    public delegate void InputAction(ConsoleKeyInfo? input, Action loopStopper);

    public delegate void RenderAction(Action loopStopper);

    public delegate void UpdateAction(long deltaMs, Action loopStopper);

    public MainLoop(InputAction inputAction, UpdateAction updateAction, RenderAction renderAction)
    {
        _inputAction = inputAction;
        _updateAction = updateAction;
        _renderAction = renderAction;
        _stopwatch = new Stopwatch();
    }

    private readonly InputAction _inputAction;
    private readonly RenderAction _renderAction;
    private readonly Stopwatch _stopwatch;
    private readonly UpdateAction _updateAction;
    private bool _isRunning;
    private long _previousDeltaMs;

    public void Start()
    {
        _isRunning = true;
        while (_isRunning)
        {
            _stopwatch.Restart();
            CallInputAction();
            CallUpdateAction();
            CallRenderAction();
            _stopwatch.Stop();
            _previousDeltaMs = _stopwatch.ElapsedMilliseconds;
        }
    }

    private void LoopStopper() =>
        _isRunning = false;

    private void CallInputAction() =>
        _inputAction(GetInputIfAny(), LoopStopper);

    private void CallUpdateAction()
    {
        const long maxStepMs = 20;
        long deltaMs = _previousDeltaMs;
        while (deltaMs > 0)
        {
            long stepMs = Math.Min(deltaMs, maxStepMs);
            _updateAction(stepMs, LoopStopper);
            deltaMs -= stepMs;
        }
    }

    private void CallRenderAction() =>
        _renderAction(LoopStopper);

    private static ConsoleKeyInfo? GetInputIfAny()
    {
        if (Console.KeyAvailable)
        {
            return Console.ReadKey(true);
        }
        return null;
    }
}
