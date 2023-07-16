using System;

using Alitz.Collections;

namespace Alitz.UnitTests;
public class MatrixTests
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
        Assert.Equal(width * height, new Matrix<char>(width, height).Count);

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
        var matrix = new Matrix<char>(width, height);
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
                _ = new Matrix<char>(width, height);
            });
}
