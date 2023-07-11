namespace Alitz.Collections;
public class IdPool : Pool<Id>
{
    protected override Id Reuse(Id toBeReused) =>
        new(toBeReused.Index, toBeReused.Version + 1);

    protected override Id Next(Id last) =>
        new(last.Index + 1, Id.MinVersion);

    protected override Id New() =>
        new(Id.MinIndex, Id.MinVersion);
}
