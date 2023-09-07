namespace Alitz;
internal class InputPump
{
    public InputPump(EntityComponentSystem ecs, IInputProducer inputProducer, IInputForwarder inputForwarder)
    {
        _ecs = ecs;
        _inputForwarder = inputForwarder;
        _inputProducer = inputProducer;
    }

    private readonly EntityComponentSystem _ecs;
    private readonly IInputForwarder _inputForwarder;
    private readonly IInputProducer _inputProducer;

    public void Advance() =>
        _inputForwarder.Forward(_inputProducer, _ecs);
}
