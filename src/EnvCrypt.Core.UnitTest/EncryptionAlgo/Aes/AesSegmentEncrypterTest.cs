using EnvCrypt.Core.EncryptionAlgo;
using EnvCrypt.Core.EncryptionAlgo.Aes;
using EnvCrypt.Core.EncryptionAlgo.Aes.Key;
using Moq;
using NUnit.Framework;

namespace EnvCrypt.Core.UnitTest.EncryptionAlgo.Aes
{
    [TestFixture]
    public class AesSegmentEncrypterTest
    {
        [Test]
        public void Given_AnyDataToEncrypt_When_Encrypt_Then_AllDataEncryptedTogether(
            [Range(0, 10000, 1000)] int actualKeySize)
        {
            // Arrange
            var algo = new Mock<IEncryptionAlgo<AesKey>>();
            var key = new AesKey();
            var dataToEncrypt = RandomByteArrayUtils.CreateRandomByteArray(actualKeySize);

            // Act
            var segEncrypter = new AesSegmentEncrypter(algo.Object);
            segEncrypter.Encrypt(dataToEncrypt, key);

            // Assert
            algo.Verify(a => a.Encrypt(dataToEncrypt, key), Times.Once());
        }
    }
}