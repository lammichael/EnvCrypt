using EnvCrypt.Core.Key.Aes;
using EnvCrypt.Core.Key.Aes.Utils;
using FluentAssertions;
using NUnit.Framework;

namespace EnvCrypt.Core.UnitTest.Key.Aes.Utils
{
    [TestFixture]
    public class AesKeySizeUtilsTest
    {
        [Test]
        public void Given_ValidKeySize_When_AESKeyGenerated_Then_GetKeySizeMustReturnActualKeySize(
            [Range(128,256,64)] int actualKeySize)
        {
            // Arrange
            var newKey = new AesKeyGenerator().GetNewKey(new AesKeyGenerationOptions()
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