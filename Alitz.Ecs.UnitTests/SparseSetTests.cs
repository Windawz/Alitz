using Alitz.Ecs.Collections;

namespace Alitz.Ecs.UnitTests;
public class SparseSetTests
{
    public SparseSetTests()
    {
        _set = new SparseSet<int>(IndexExtractors.Int32IndexExtractor);
    }

    private readonly ISparseSet<int> _set;
}
