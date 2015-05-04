using EnvCrypt.Core.EncryptionAlgo.Rsa;
using FluentAssertions;
using NUnit.Framework;

namespace EnvCrypt.Core.UnitTest.EncryptionAlgo.Rsa
{
    [TestFixture]
    public class RsaMaxEncryptSizeUtilsTest
    {
        [Test]
        public void Given_ValidKey_When_GetKeySize_Then_CorrectKeySizeReturned()
        {
            // Arrange
            const int keySize = 384;
            var generator = new RsaKeyGenerator();
            var aNewKey = generator.GetNewKey(new RsaGenerationOptions(keySize, true));

            // Act
            var actualKeySize = aNewKey.GetKeySize();

            // Assert
            actualKeySize.Should().Be(keySize);
        }


        [Test]
        public void Given_ValidKeyWithOAEP_When_GetMaxBytes_CorrectMaxBytesReturned()
        {
            // Arrange
            const int keySize = 384;
            var generator = new RsaKeyGenerator();
            var aNewKey = generator.GetNewKey(new RsaGenerationOptions(keySize, true));

            // Act
            var actualMaxBytes = aNewKey.GetMaxBytesThatCanBeEncrypted();

            // Assert
            actualMaxBytes.Should().Be(((keySize - 384)/8) + 7);
        }


        [Test]
        public void Given_ValidKeyWithoutOAEP_When_GetMaxBytes_CorrectMaxBytesReturned()
        {
            // Arrange
            const int keySize = 384;
            var generator = new RsaKeyGenerator();
            var aNewKey = generator.GetNewKey(new RsaGenerationOptions(keySize, false));

            // Act
            var actualMaxBytes = aNewKey.GetMaxBytesThatCanBeEncrypted();

            // Assert
            actualMaxBytes.Should().Be(((keySize - 384) / 8) + 37);
        }
    }
}