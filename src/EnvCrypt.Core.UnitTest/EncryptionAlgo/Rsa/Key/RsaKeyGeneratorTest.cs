using EnvCrypt.Core.EncryptionAlgo.Rsa.Key;
using EnvCrypt.Core.EncryptionAlgo.Rsa.Utils;
using FluentAssertions;
using NUnit.Framework;

namespace EnvCrypt.Core.UnitTest.EncryptionAlgo.Rsa.Key
{
    [TestFixture]
    class RsaKeyGeneratorTest
    {
        [Test]
        public void Given_ValidKeyGenerationOptions_When_GetNewKey_Then_PrivateKeyReturned(
            [Range(384, 1024, 8)] int actualKeySize)
        {
            // Arrange
            var generator = new RsaKeyGenerator();

            // Act
            var newKey = generator.GetNewKey(new RsaKeyGenerationOptions(actualKeySize, true));

            // Assert
            newKey.Key.Should().NotBeNull();
            newKey.UseOaepPadding.Should().BeTrue();
        }


        [Test]
        public void Given_KeySize_When_GetNewKey_Then_KeySizeMatches(
            [Range(384, 1024, 8)] int actualKeySize)
        {
            // Arrange
            var generator = new RsaKeyGenerator();

            // Act
            var newKey = generator.GetNewKey(new RsaKeyGenerationOptions(actualKeySize, true));

            // Assert
            newKey.GetKeySize().Should().Be(actualKeySize);
        }
    }
}
