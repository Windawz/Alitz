using System;

namespace Alitz.Collections;
public class IndexExtractor<T>
{
    public IndexExtractor(Func<T, int> extractorFunc)
    {
        _extractorFunc = extractorFunc;
    }

    private readonly Func<T, int> _extractorFunc;

    public int Extract(T value)
    {
        int index = _extractorFunc(value);
        if (index < 0)
        {
            throw new NegativeIndexExtractedException(_extractorFunc);
        }
        return index;
    }
}
