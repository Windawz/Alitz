using Alitz.Ecs;

namespace Alitz.Engine;
internal interface IInputForwarder
{
    void Forward(IInputProducer producer, EntityComponentSystem ecs);
}
