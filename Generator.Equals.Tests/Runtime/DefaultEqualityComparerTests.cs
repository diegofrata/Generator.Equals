using FluentAssertions;
using Xunit;

namespace Generator.Equals.Tests.Runtime;

public partial class DefaultEqualityComparerTests
{
    [Equatable]
    sealed partial class SealedClass
    {
    }

    [Equatable]
    partial class NonSealedClass
    {
    }


    [Fact]
    public void GetHashCode_SealedClass_NullValueShouldReturnZero()
    {
        DefaultEqualityComparer<SealedClass>.Default.GetHashCode(null!).Should().Be(0);
    }


    [Fact]
    public void GetHashCode_NonSealedClass_NullValueShouldReturnZero()
    {
        DefaultEqualityComparer<NonSealedClass>.Default.GetHashCode(null!).Should().Be(0);
    }
}