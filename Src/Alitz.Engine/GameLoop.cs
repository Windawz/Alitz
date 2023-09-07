using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Alitz;
internal class GameLoop
{
    public GameLoop()
    {
        _iterationContext = new IterationContext(0, () => IsRunning, isRunning => IsRunning = isRunning);
        _stopwatch = new Stopwatch();
    }

    private readonly IterationContext _iterationContext;
    private readonly List<GameLoopIterationStartedHandler> _iterationStartedHandlers = new(2);
    private readonly Stopwatch _stopwatch;

    public bool IsRunning { get; set; }

    public event GameLoopIterationStartedHandler IterationStarted
    {
        add => _iterationStartedHandlers.Add(value);
        remove => _iterationStartedHandlers.Remove(value);
    }

    public void Start()
    {
        IsRunning = true;
        while (IsRunning)
        {
            _stopwatch.Restart();
            OnIterationStarted();
            _stopwatch.Stop();
            _iterationContext.DeltaMs = _stopwatch.ElapsedMilliseconds;
        }
    }

    private void OnIterationStarted()
    {
        const long maxStepMs = 20;
        long deltaMs = _iterationContext.DeltaMs;
        while (deltaMs > 0)
        {
            long stepMs = Math.Min(deltaMs, maxStepMs);
            _iterationContext.DeltaMs = stepMs;
            for (int i = 0; i < _iterationStartedHandlers.Count; i++)
            {
                _iterationStartedHandlers[i](_iterationContext);
            }
            deltaMs -= stepMs;
        }
    }

    private class IterationContext : IGameLoopIterationContext
    {
        public IterationContext(long initialDeltaMs, Func<bool> isRunningGetter, Action<bool> isRunningSetter)
        {
            DeltaMs = initialDeltaMs;
            _isRunningGetter = isRunningGetter;
            _isRunningSetter = isRunningSetter;
        }

        private readonly Func<bool> _isRunningGetter;
        private readonly Action<bool> _isRunningSetter;

        public long DeltaMs { get; set; }
        public bool IsRunning
        {
            get => _isRunningGetter();
            set => _isRunningSetter(value);
        }
    }
}
