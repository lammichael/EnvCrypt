using System.Text;
using EnvCrypt.Core.EncryptionAlgo.Aes;
using EnvCrypt.Core.EncryptionAlgo.Rsa;
using EnvCrypt.Core.Key.Aes;
using FluentAssertions;
using NUnit.Framework;

namespace EnvCrypt.Core.UnitTest.EncryptionAlgo.Aes
{
    [TestFixture]
    public class AesAlgoTest
    {
        [Test]
        public void Given_ValidKeyAndBinaryData_When_EncryptAndDecrypt_Then_DecryptionIsSameResult(
            [Values(192,256)] int keySize)
        {
            // Arrange
            const string strToTestWith = "string should be encrypted and come out the same after descryption";
            var byteConverter = new UnicodeEncoding();
            var strAsBytes = byteConverter.GetBytes(strToTestWith);

            var algo = new AesAlgo();

            // Act
            var key = new AesKeyGenerator().GetNewKey(new AesKeyGenerationOptions()
            {
                KeySize = keySize
            });
            var result = algo.Decrypt(algo.Encrypt(strAsBytes, key), key);

            // Assert
            byteConverter.GetString(result).Should().Be(strToTestWith);
        }
    }
}