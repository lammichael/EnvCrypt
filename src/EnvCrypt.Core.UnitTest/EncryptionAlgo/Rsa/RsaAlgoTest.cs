using System.Text;
using EnvCrypt.Core.EncryptionAlgo.Rsa;
using FluentAssertions;
using NUnit.Framework;

namespace EnvCrypt.Core.UnitTest.EncryptionAlgo.Rsa
{
    [TestFixture]
    class RsaAlgoTest
    {
        [Test]
        public void Given_ValidKeyAndBinaryData_When_EncryptAndDecrypt_Then_DecryptionIsSameResult(
            [Values(2040, 2048,2056)] int keySize)
        {
            // Arrange
            const string strToTestWith = "encrypt this string and get the same after decryption";
            var byteConverter = new UnicodeEncoding();
            var strAsBytes = byteConverter.GetBytes(strToTestWith);

            var algo = new RsaAlgo();

            // Act
            var key = new RsaKeyGenerator().GetNewKey(new RsaGenerationOptions(keySize, true));
            var result = algo.Decrypt(algo.Encrypt(strAsBytes, key), key);

            // Assert
            byteConverter.GetString(result).Should().Be(strToTestWith);
        }
    }
}
