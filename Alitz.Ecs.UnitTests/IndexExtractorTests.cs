using System;

namespace Alitz.UnitTests;
public class IndexExtractorTests
{
    public class NegativeReturnValueTests
    {
        public NegativeReturnValueTests()
        {
            _entitySpace = new EntitySpace();
            _extractorFunc = _ => -1;
        }

        private readonly EntitySpace _entitySpace;
        private readonly Func<Entity, int> _extractorFunc;

        [Fact]
        public void ThrowsWhenResultingIndexIsNegative()
        {
            var extractor = new IndexExtractor<Entity>(_extractorFunc);
            Assert.Throws<NegativeIndexExtractedException>(() => extractor.Extract(_entitySpace.Create()));
        }
    }
}
