using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Extensions;

namespace Lsquared.Extensions.Enums.Tests
{
    public class SetTests
    {
        [Fact]
        public void Empty()
        {
            // Arrange

            // Act
            var set0 = Set<Foo>.Empty;
            var set1 = new Set<Foo>();

            // Assert
            Assert.Equal(set0, set1);
        }

        [Fact]
        public void IsEmpty()
        {
            // Arrange
            var set = new Set<Foo>();

            // Act
            var actual = set.IsEmpty;

            // Assert
            Assert.Equal(true, actual);
        }

        [Fact]
        public void Include()
        {
            // Arrange
            var set = new Set<Foo>();

            // Act
            var actual = set.Include(Foo.First).Include(Foo.Fifty);

            // Assert
            Assert.Equal(false, actual.Contains(Foo.None));
            Assert.Equal(true, actual.Contains(Foo.First));
            Assert.Equal(false, actual.Contains(Foo.Second));
            Assert.Equal(true, actual.Contains(Foo.Fifty));
        }

        [Fact]
        public void Exclude()
        {
            // Arrange
            var set = new Set<Foo>().Include(Foo.None).Include(Foo.First).Include(Foo.Second).Include(Foo.Fifty);

            // Act
            var actual = set.Exclude(Foo.First).Exclude(Foo.Fifty);

            // Assert
            Assert.Equal(true, actual.Contains(Foo.None));
            Assert.Equal(false, actual.Contains(Foo.First));
            Assert.Equal(true, actual.Contains(Foo.Second));
            Assert.Equal(false, actual.Contains(Foo.Fifty));
        }

        [Theory]
        [MemberData(nameof(ToStringData))]
        public void ToString(Set<Foo> set, string expected)
        {
            // Arrange

            // Act
            var actual = set.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void A()
        {
            // Arrange

            // Act
            var set = new Set<Foo>(Foo.None);

            // Assert
            Assert.Equal(true, set.Contains(Foo.None));
        }

        [Fact]
        public void B()
        {
            // Arrange

            // Act
            var set = new Set<Foo>(Foo.First, Foo.Second);

            // Assert
            Assert.Equal(false, set.Contains(Foo.None));
            Assert.Equal(true, set.Contains(Foo.First));
            Assert.Equal(true, set.Contains(Foo.Second));
            Assert.Equal(false, set.Contains(Foo.Fifty));
        }

        [Fact]
        public void C()
        {
            // Arrange

            // Act
            var set = new Set<Foo>(Foo.None, Foo.Second, Foo.Fifty);

            // Assert
            Assert.Equal(true, set.Contains(Foo.None));
            Assert.Equal(false, set.Contains(Foo.First));
            Assert.Equal(true, set.Contains(Foo.Second));
            Assert.Equal(true, set.Contains(Foo.Fifty));
        }

        public static IEnumerable<object[]> ToStringData()
        {
            yield return new object[] { Set<Foo>.Empty, "[]" };
            yield return new object[] { Set<Foo>.Empty.Include(Foo.None), "[None]" };
            yield return new object[] { Set<Foo>.Empty.Include(Foo.None).Include(Foo.Second), "[None, Second]" };
            yield return new object[] { Set<Foo>.Empty.Include(Foo.None).Include(Foo.First).Include(Foo.Second).Include(Foo.Fifty), "[None, First, Second, Fifty]" };
        }
    }
}
