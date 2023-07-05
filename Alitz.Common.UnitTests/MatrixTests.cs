﻿using System;

using Alitz.Collections;

namespace Alitz.UnitTests;
public static class MatrixTests
{
    public abstract class GenericTests<T>
    {
        [Theory]
        [InlineData(2, 6)]
        [InlineData(0, 0)]
        [InlineData(1, 1)]
        [InlineData(0, 1)]
        [InlineData(1, 0)]
        [InlineData(1, 3)]
        [InlineData(100, 200)]
        public void IsSizeProperlyDependentOnElementCount(int width, int height) =>
            Assert.Equal(width * height, new Matrix<T>(width, height).Count);

        [Theory]
        [InlineData(2, 6)]
        [InlineData(0, 0)]
        [InlineData(1, 1)]
        [InlineData(0, 1)]
        [InlineData(1, 0)]
        [InlineData(1, 3)]
        [InlineData(100, 200)]
        public void IsSizeProperlyInitialized(int width, int height)
        {
            var matrix = new Matrix<T>(width, height);
            Assert.Equal(width, matrix.Width);
            Assert.Equal(height, matrix.Height);
        }

        [Theory]
        [InlineData(-1, 0)]
        [InlineData(0, -1)]
        [InlineData(-1, -1)]
        [InlineData(-34, -20)]
        public void DoesThrowOnInvalidSize(int width, int height) =>
            Assert.ThrowsAny<ArgumentException>(
                () =>
                {
                    _ = new Matrix<T>(width, height);
                });
    }

    public abstract class GenericReferenceTests<T> : GenericTests<T> where T : class { }

    public class CharTests : GenericTests<char> { }

    public class Int32Tests : GenericTests<int> { }

    public class StringTests : GenericReferenceTests<string> { }
}
