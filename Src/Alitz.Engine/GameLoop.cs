using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Alitz.Engine;
internal class GameLoop
{
    public GameLoop(IterationHandler iterationHandler)
    {
        _stopwatch = new Stopwatch();
        _iterationHandler = iterationHandler;
    }

    private const long _maxStepMs = 20;
    private readonly Stopwatch _stopwatch;
    private readonly IterationHandler _iterationHandler;
    private long _lastDeltaMs = 0;

    public bool IsRunning { get; set; }

    public void Start()
    {
        IsRunning = true;
        while (IsRunning)
        {
            _stopwatch.Restart();

            long deltaMs = _lastDeltaMs;
            while (deltaMs > 0)
            {
                long stepMs = Math.Min(deltaMs, _maxStepMs);
                _iterationHandler(this, stepMs);
                deltaMs -= stepMs;
            }

            _stopwatch.Stop();
            _lastDeltaMs = _stopwatch.ElapsedMilliseconds;
        }
    }

    public delegate void IterationHandler(GameLoop loop, long deltaMs);
}
