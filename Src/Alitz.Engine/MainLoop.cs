using System;
using System.Diagnostics;

namespace Alitz;
internal class MainLoop
{
    public delegate void InputAction(ConsoleKeyInfo? input);

    public delegate void RenderAction();

    public delegate void UpdateAction(long deltaMs);

    public MainLoop(InputAction? inputAction = null, UpdateAction? updateAction = null, RenderAction? renderAction = null)
    {
        _inputAction = inputAction ?? delegate { };
        _updateAction = updateAction ?? delegate { };
        _renderAction = renderAction ?? delegate { };
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

    public void Stop() =>
        _isRunning = false;

    private void CallInputAction() =>
        _inputAction(GetInputIfAny());

    private void CallUpdateAction()
    {
        const long maxStepMs = 20;
        long deltaMs = _previousDeltaMs;
        while (deltaMs > 0)
        {
            long stepMs = Math.Min(deltaMs, maxStepMs);
            _updateAction(stepMs);
            deltaMs -= stepMs;
        }
    }

    private void CallRenderAction() =>
        _renderAction();

    private static ConsoleKeyInfo? GetInputIfAny()
    {
        if (Console.KeyAvailable)
        {
            return Console.ReadKey(true);
        }
        return null;
    }
}
