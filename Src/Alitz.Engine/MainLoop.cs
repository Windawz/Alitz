using System;
using System.Diagnostics;

namespace Alitz;
internal class MainLoop
{
    public delegate void InputCheckedEventHandler(ConsoleKeyInfo? input);

    public delegate void RenderStartedEventHandler();

    public delegate void UpdateStartedEventHandler(long deltaMs);

    public MainLoop()
    {
        _stopwatch = new Stopwatch();
    }

    private readonly Stopwatch _stopwatch;
    private bool _isRunning;
    private long _previousDeltaMs;

    public event InputCheckedEventHandler? InputChecked;
    public event UpdateStartedEventHandler? UpdateStarted;
    public event RenderStartedEventHandler? RenderStarted;

    public void Start()
    {
        _isRunning = true;
        while (_isRunning)
        {
            _stopwatch.Restart();
            OnInputChecked();
            OnUpdateStarted();
            OnRenderStarted();
            _stopwatch.Stop();
            _previousDeltaMs = _stopwatch.ElapsedMilliseconds;
        }
    }

    public void Stop() =>
        _isRunning = false;

    private void OnInputChecked() =>
        InputChecked?.Invoke(GetInputIfAny());

    private void OnUpdateStarted()
    {
        const long maxStepMs = 20;
        long deltaMs = _previousDeltaMs;
        while (deltaMs > 0)
        {
            long stepMs = Math.Min(deltaMs, maxStepMs);
            UpdateStarted?.Invoke(stepMs);
            deltaMs -= stepMs;
        }
    }

    private void OnRenderStarted() =>
        RenderStarted?.Invoke();

    private static ConsoleKeyInfo? GetInputIfAny()
    {
        if (Console.KeyAvailable)
        {
            return Console.ReadKey(true);
        }
        return null;
    }
}
