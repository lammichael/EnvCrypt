using System.Diagnostics;
using System.Text;
using EnvCrypt.Core.EncrypedData.UserStringConverter;
using EnvCrypt.Core.EncryptionAlgo.Rsa;
using EnvCrypt.Core.Key.Rsa;
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
            var converter = new Utf16LittleEndianUserStringConverter();
            var strAsBytes = converter.Encode(strToTestWith);

            var key = new RsaKeyGenerator().GetNewKey(new RsaKeyGenerationOptions()
            {
                NewKeyName = "test",
                KeySize = keySize,
                UseOaepPadding = true
            });

            // Act
            var algo = new RsaAlgo();
            var result = algo.Decrypt(algo.Encrypt(strAsBytes, key), key);

            // Assert
            converter.Decode(result).Should().Be(strToTestWith);
        }
    }
}
