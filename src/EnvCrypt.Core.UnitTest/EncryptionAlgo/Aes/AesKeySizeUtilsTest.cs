using EnvCrypt.Core.EncryptionAlgo.Aes;
using FluentAssertions;
using NUnit.Framework;

namespace EnvCrypt.Core.UnitTest.EncryptionAlgo.Aes
{
    [TestFixture]
    public class AesKeySizeUtilsTest
    {
        [Test]
        public void Given_ValidKeySize_When_AESKeyGenerated_Then_GetKeySizeMustReturnActualKeySize(
            [Range(128,256,64)] int actualKeySize)
        {
            // Arrange
            var newKey = new AesKeyGenerator().GetNewKey(new AesGenerationOptions()
            {
                KeySize = actualKeySize
            });

            // Act
            var expectedKeySize = newKey.GetKeySize();

            // Assert
            expectedKeySize.Should().Be(actualKeySize);
        }
    }
}