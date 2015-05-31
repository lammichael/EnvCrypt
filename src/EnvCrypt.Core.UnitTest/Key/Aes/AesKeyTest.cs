using System.Diagnostics;
using EnvCrypt.Core.Key.Aes;
using FluentAssertions;
using NUnit.Framework;

namespace EnvCrypt.Core.UnitTest.Key.Aes
{
    [TestFixture]
    public class AesKeyTest
    {
        [Test]
        public void Given_TwoKeysWithSameKeyAndIVValues_When_GetHashCode_Then_HashCodesMatch()
        {
            // Arrange
            var key1 = new AesKey()
            {
                Iv = new byte[] {1, 2},
                Key = new byte[] {1, 2, 3},
            };
            var key2 = new AesKey()
            {
                Iv = new byte[] { 1, 2 },
                Key = new byte[] { 1, 2, 3 },
            };

            // Act
            // Assert
            Debug.WriteLine("key1 hash: {0}", key1.GetHashCode());
            Debug.WriteLine("key2 hash: {0}", key2.GetHashCode());
            key1.GetHashCode().Should().Be(key2.GetHashCode());
        }
    }
}