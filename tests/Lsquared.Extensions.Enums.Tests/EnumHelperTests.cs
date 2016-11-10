using Xunit;

namespace Lsquared.Extensions.Enums.Tests
{
    public class EnumHelperTests
    {
        [Theory]
        [InlineData(new object[] { Foo.None, nameof(Foo.None) })]
        [InlineData(new object[] { Foo.First, nameof(Foo.First) })]
        [InlineData(new object[] { Foo.Second, nameof(Foo.Second) })]
        [InlineData(new object[] { Foo.Fifty, nameof(Foo.Fifty) })]
        public void GetName_WithEnumValue_DoesNotThrow(Foo value, string expected)
        {
            // Arrange

            // Act
            var actual = EnumHelper<Foo>.GetName(value);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(new object[] { Foo.None, 0 })]
        [InlineData(new object[] { Foo.First, 1 })]
        [InlineData(new object[] { Foo.Second, 2 })]
        [InlineData(new object[] { Foo.Fifty, 3 })]
        public void GetOrdinal_WithEnumValue_DoesNotThrow(Foo value, int expected)
        {
            // Arrange

            // Act
            var actual = EnumHelper<Foo>.GetOrdinal(value);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(new object[] { Foo.None, 0 })]
        [InlineData(new object[] { Foo.First, 1 })]
        [InlineData(new object[] { Foo.Second, 2 })]
        [InlineData(new object[] { Foo.Fifty, 5 })]
        public void GetValue_WithEnumValue_DoesNotThrow(Foo value, int expected)
        {
            // Arrange

            // Act
            var actual = EnumHelper<Foo>.GetValue(value);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Length_WithEnum_DoesNotThrow()
        {
            // Arrange
            var expected = 4;

            // Act
            var actual = EnumHelper<Foo>.Count;

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
