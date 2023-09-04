using System;
using System.Diagnostics;

namespace Alitz;
internal class MainLoop
{
    public delegate void InputAction(ConsoleKeyInfo? input, Action loopStopper);

    public delegate void RenderAction(Action loopStopper);

    public delegate void UpdateAction(long deltaMs, Action loopStopper);

    private MainLoop(InputAction? inputAction, UpdateAction? updateAction, RenderAction? renderAction)
    {
        _inputAction = inputAction;
        _updateAction = updateAction;
        _renderAction = renderAction;
        _stopwatch = new Stopwatch();
    }

    private readonly InputAction? _inputAction;
    private readonly RenderAction? _renderAction;
    private readonly Stopwatch _stopwatch;
    private readonly UpdateAction? _updateAction;
    private bool _isRunning;
    private long _previousDeltaMs;

    public static Builder CreateBuilder() =>
        new();

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

    private void CallInputAction()
    {
        if (_inputAction is not null)
        {
            _inputAction(GetInputIfAny(), LoopStopper);
        }
    }

    private void CallUpdateAction()
    {
        if (_updateAction is not null)
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
    }

    private void CallRenderAction()
    {
        if (_renderAction is not null)
        {
            _renderAction(LoopStopper);
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

    public class Builder
    {
        internal Builder() { }

        private InputAction? _inputAction;
        private RenderAction? _renderAction;
        private UpdateAction? _updateAction;

        public Builder SetInputAction(InputAction inputAction)
        {
            _inputAction = inputAction;
            return this;
        }

        public Builder SetRenderAction(RenderAction renderAction)
        {
            _renderAction = renderAction;
            return this;
        }

        public Builder SetUpdateAction(UpdateAction updateAction)
        {
            _updateAction = updateAction;
            return this;
        }

        public MainLoop Build() =>
            new(_inputAction, renderAction: _renderAction, updateAction: _updateAction);
    }
}
