using EnvCrypt.Core.EncryptionAlgo;
using EnvCrypt.Core.EncryptionAlgo.Aes;
using EnvCrypt.Core.Key.Aes;
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
            algo.Setup(encryptionAlgo => encryptionAlgo.Encrypt(It.IsAny<byte[]>(), key)).Returns(new byte[1]);
            var dataToEncrypt = RandomByteArrayUtils.CreateRandomByteArray(actualKeySize);

            // Act
            var segEncrypter = new AesSegmentEncryptionAlgo(algo.Object);
            segEncrypter.Encrypt(dataToEncrypt, key);

            // Assert
            algo.Verify(a => a.Encrypt(dataToEncrypt, key), Times.Once());
        }
    }
}