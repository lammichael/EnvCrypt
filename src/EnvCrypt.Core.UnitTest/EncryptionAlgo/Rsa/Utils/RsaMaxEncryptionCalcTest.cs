using EnvCrypt.Core.EncryptionAlgo.Rsa;
using EnvCrypt.Core.EncryptionAlgo.Rsa.Key;
using EnvCrypt.Core.EncryptionAlgo.Rsa.Utils;
using FluentAssertions;
using NUnit.Framework;

namespace EnvCrypt.Core.UnitTest.EncryptionAlgo.Rsa.Utils
{
    [TestFixture]
    public class RsaMaxEncryptionCalcTest
    {
        [Test]
        public void Given_ValidKeyWithOAEP_When_GetMaxBytes_CorrectMaxBytesReturned(
            [Values(384, 2048, 2056)] int keySize)
        {
            // Arrange
            var generator = new RsaKeyGenerator();
            var aNewKey = generator.GetNewKey(new RsaKeyGenerationOptions(keySize, true));

            // Act
            var actualMaxBytes = new RsaMaxEncryptionCalc().GetMaxBytesThatCanBeEncrypted(aNewKey);

            // Assert
            actualMaxBytes.Should().Be(((keySize - 384) / 8) + 7);
        }


        [Test]
        public void Given_ValidKeyWithoutOAEP_When_GetMaxBytes_CorrectMaxBytesReturned(
            [Values(384, 2048, 2056)] int keySize)
        {
            // Arrange
            var generator = new RsaKeyGenerator();
            var aNewKey = generator.GetNewKey(new RsaKeyGenerationOptions(keySize, false));

            // Act
            var actualMaxBytes = new RsaMaxEncryptionCalc().GetMaxBytesThatCanBeEncrypted(aNewKey);

            // Assert
            actualMaxBytes.Should().Be(((keySize - 384) / 8) + 37);
        }
    }
}