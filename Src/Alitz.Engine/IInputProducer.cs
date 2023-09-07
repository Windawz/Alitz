namespace Alitz;
internal interface IInputProducer
{
    InputPollResult Poll(out Input? input);
}
