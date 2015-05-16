using EnvCrypt.Core.EncryptionAlgo.Rsa;
using EnvCrypt.Core.EncryptionAlgo.Rsa.Key;
using EnvCrypt.Core.EncryptionAlgo.Rsa.Utils;
using FluentAssertions;
using NUnit.Framework;

namespace EnvCrypt.Core.UnitTest.EncryptionAlgo.Rsa.Utils
{
    [TestFixture]
    public class RsaKeySizeUtilsTest
    {
        [Test]
        public void Given_ValidKey_When_GetKeySize_Then_CorrectKeySizeReturned(
            [Values(384, 2048, 2056)] int keySize)
        {
            // Arrange
            var generator = new RsaKeyGenerator();
            var aNewKey = generator.GetNewKey(new RsaKeyGenerationOptions(keySize, true));

            // Act
            var actualKeySize = aNewKey.GetKeySize();

            // Assert
            actualKeySize.Should().Be(keySize);
        }
    }
}