namespace Alitz;
internal interface IInputForwarder
{
    void Forward(IInputProducer producer, EntityComponentSystem ecs);
}
