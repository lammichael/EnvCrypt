using System.Security.Cryptography;
using EnvCrypt.Core.Key.XmlPoco;
using EnvCrypt.Core.Utils;
using FluentAssertions;
using NUnit.Framework;

namespace EnvCrypt.Core.UnitTest.Utils
{
    [TestFixture]
    public class XmlToAndFromPocoTest
    {
        [Test]
        public void Given_ValidAESKeyPOCO_When_SerializeAndDeserialize_Then_POCOContentIsTheSame()
        {
            // Arrange
            var aesKeyXmlPoco = new EnvCryptKey()
            {
                Name = "My AES Key",
                Aes = new[]
                {
                    new EnvCryptKeyAes()
                    {
                        Iv = "aesIV",
                        Key = "aesKey"
                    }
                },
                Encryption = "Aes",
                Type = null
            };

            // Act
            var xmlUtil = new XmlSerializationUtils<EnvCryptKey>();
            var serialized = xmlUtil.Serialize(aesKeyXmlPoco);
            var deserialized = xmlUtil.Deserialize(serialized);

            // Assert
            deserialized.Should().NotBeNull();
            deserialized.Name.Should().Be(aesKeyXmlPoco.Name);
            deserialized.Encryption.Should().Be(aesKeyXmlPoco.Encryption);
            deserialized.Type.Should().BeNull();
            deserialized.Aes.Should().HaveCount(1);
            deserialized.Aes[0].Iv.Should().Be("aesIV");
            deserialized.Aes[0].Key.Should().Be("aesKey");
        }
    }
}