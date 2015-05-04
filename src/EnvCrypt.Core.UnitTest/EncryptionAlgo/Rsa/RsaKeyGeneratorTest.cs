using EnvCrypt.Core.EncryptionAlgo.Rsa;
using FluentAssertions;
using NUnit.Framework;

namespace EnvCrypt.Core.UnitTest.EncryptionAlgo.Rsa
{
    [TestFixture]
    class RsaKeyGeneratorTest
    {
        [Test]
        public void Given_ValidKeyGenerationOptions_When_GetNewKey_Then_PrivateKeyReturned()
        {
            // Arrange
            var generator = new RsaKeyGenerator();

            // Act
            var newKey = generator.GetNewKey(new RsaGenerationOptions(384, true));

            // Assert
            newKey.Key.Should().NotBeNull();
            newKey.UseOaepPadding.Should().BeTrue();
        }


        [Test]
        public void Given_KeySize_When_GetNewKey_Then_KeySizeMatches()
        {
            // Arrange
            var generator = new RsaKeyGenerator();
            const int desiredKeySize = 384;

            // Act
            var newKey = generator.GetNewKey(new RsaGenerationOptions(desiredKeySize, true));

            // Assert
            newKey.GetKeySize().Should().Be(desiredKeySize);
        }
    }
}
