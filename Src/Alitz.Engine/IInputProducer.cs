namespace Alitz.Engine;
internal interface IInputProducer
{
    InputPollResult Poll(out Input? input);
}
