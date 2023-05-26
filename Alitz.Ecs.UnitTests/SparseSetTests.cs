using System;

using Alitz.UnitTests.Utilities;

namespace Alitz.Ecs.UnitTests; 
public class SparseSetTests {
    public class EmptySetTests {
        public EmptySetTests() {
            _set = new SparseSet<int, Int32IndexProvider>();
        }
        
        private readonly SparseSet<int, Int32IndexProvider> _set;
        
        [Fact]
        public void SetCountIsGreaterThanZeroAfterAdding() {
            _set.Add(42);
            Assert.Equal(1, _set.Count);
        }
        
        [Fact]
        public void SetContainsAddedElement() {
            const int element = 42;
            _set.Add(element);
            Assert.True(_set.Contains(element));
        }
        
        [Fact]
        public void SetDoesNotContainUnrelatedElement() {
            _set.Add(42);
            Assert.False(_set.Contains(63));
        }
        
        [Fact]
        public void SetAdditionOrderIsMaintained() {
            const int firstElement = 42;
            const int secondElement = 63;
            _set.Add(firstElement);
            Assert.Equal(firstElement, _set.Keys[0]);
            _set.Add(secondElement);
            Assert.Equal(firstElement, _set.Keys[0]);
            Assert.Equal(secondElement, _set.Keys[1]);
        }
    }
    
    public class SingleElementSetTests {
        private const int Element = 42;
        
        public SingleElementSetTests() {
            _set = new SparseSet<int, Int32IndexProvider>();
            _set.Add(Element);
        }
        
        private readonly SparseSet<int, Int32IndexProvider> _set;
        
        [Fact]
        public void SetCountIsZeroOnElementRemoved() {
            _set.Remove(Element);
            Assert.Equal(0, _set.Count);
        }
        
        [Fact]
        public void RemovingElementReturnsTrue() {
            Assert.True(_set.Remove(Element));
        }
        
        [Fact]
        public void RemovingUnrelatedElementReturnsFalse() {
            Assert.False(_set.Remove(63));
        }
        
        [Fact]
        public void SetDoesNotContainElementRemoved() {
            _set.Remove(Element);
            Assert.False(_set.Contains(Element));
        }
    }
    
    public class UniqueMultiElementSetTests {
        public UniqueMultiElementSetTests() {
            _set = new SparseSet<int, Int32IndexProvider>();
            for (int i = 0; i < 100; i++) {
                _set.Add(i);
            }
        }
        
        private readonly SparseSet<int, Int32IndexProvider> _set;
        
        [Theory]
        [RandomInt32Data(SetCount = 3, MinValue = 0, MaxValue = 99)]
        public void AttemptToAddExistingElementReturnsNegativeOne(int addedElement) {
            Assert.Equal(-1, _set.Add(addedElement));
        }
        
        [Theory]
        [RandomInt32Data(SetCount = 3, MinValue = 0, MaxValue = 99)]
        public void RemovingElementPutsTheLastElementIntoItsPosition(int index) {
            int element = _set.Keys[index];
            int lastElement = _set.Keys[^1];
            Assert.True(_set.Remove(element));
            Assert.Equal(lastElement, _set.Keys[index]);
        }
    }
}
