using System;
using System.Collections.Generic;
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

    private readonly List<InputCheckedEventHandler> _inputCheckedHandlers = new(2);
    private readonly List<RenderStartedEventHandler> _renderStartedHandlers = new(2);
    private readonly Stopwatch _stopwatch;
    private readonly List<UpdateStartedEventHandler> _updateStartedHandlers = new(2);
    private bool _isRunning;
    private long _previousDeltaMs;

    public event InputCheckedEventHandler InputChecked
    {
        add => _inputCheckedHandlers.Add(value);
        remove => _inputCheckedHandlers.Remove(value);
    }

    public event UpdateStartedEventHandler UpdateStarted
    {
        add => _updateStartedHandlers.Add(value);
        remove => _updateStartedHandlers.Remove(value);
    }

    public event RenderStartedEventHandler RenderStarted
    {
        add => _renderStartedHandlers.Add(value);
        remove => _renderStartedHandlers.Remove(value);
    }

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

    private void OnInputChecked()
    {
        for (int i = 0; i < _inputCheckedHandlers.Count; i++)
        {
            _inputCheckedHandlers[i](GetInputIfAny());
        }
    }

    private void OnUpdateStarted()
    {
        const long maxStepMs = 20;
        long deltaMs = _previousDeltaMs;
        while (deltaMs > 0)
        {
            long stepMs = Math.Min(deltaMs, maxStepMs);
            for (int i = 0; i < _updateStartedHandlers.Count; i++)
            {
                _updateStartedHandlers[i](stepMs);
            }
            deltaMs -= stepMs;
        }
    }

    private void OnRenderStarted()
    {
        for (int i = 0; i < _renderStartedHandlers.Count; i++)
        {
            _renderStartedHandlers[i]();
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
