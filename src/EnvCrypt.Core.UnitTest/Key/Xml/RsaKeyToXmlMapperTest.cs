using System;
using System.Security.Cryptography;
using EnvCrypt.Core.EncryptionAlgo;
using EnvCrypt.Core.EncryptionAlgo.Aes.Key;
using EnvCrypt.Core.EncryptionAlgo.Rsa.Key;
using EnvCrypt.Core.Key;
using EnvCrypt.Core.Key.Xml;
using EnvCrypt.Core.Utils;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace EnvCrypt.Core.UnitTest.Key.Xml
{
    [TestFixture]
    public class RsaKeyToXmlMapperTest
    {
        [Test]
        public void Given_ValidRSAKey_When_MapToXMLPOCO_Then_EncodingHelperClassMustBeused()
        {
            // Arrange
            var key = new RsaKey(new RSAParameters()
            {
                D = RandomByteArrayUtils.CreateRandomByteArray(10),
                DP = RandomByteArrayUtils.CreateRandomByteArray(11),
                DQ = RandomByteArrayUtils.CreateRandomByteArray(12),
                Exponent = RandomByteArrayUtils.CreateRandomByteArray(13),
                Modulus = RandomByteArrayUtils.CreateRandomByteArray(14),
                InverseQ = RandomByteArrayUtils.CreateRandomByteArray(15),
                P = RandomByteArrayUtils.CreateRandomByteArray(16),
                Q = RandomByteArrayUtils.CreateRandomByteArray(17),
            }, true);
            var converter = new Mock<IStringPersistConverter>(MockBehavior.Strict);
            converter.Setup(c => c.Encode(key.Key.D)).Returns("d");
            converter.Setup(c => c.Encode(key.Key.DP)).Returns("dp");
            converter.Setup(c => c.Encode(key.Key.DQ)).Returns("dq");
            converter.Setup(c => c.Encode(key.Key.Exponent)).Returns("exponent");
            converter.Setup(c => c.Encode(key.Key.Modulus)).Returns("modulus");
            converter.Setup(c => c.Encode(key.Key.InverseQ)).Returns("inverseQ");
            converter.Setup(c => c.Encode(key.Key.P)).Returns("p");
            converter.Setup(c => c.Encode(key.Key.Q)).Returns("q");

            // Act
            var aesKeyToXmlMapper = new RsaKeyToXmlMapper(converter.Object);
            var xmlPoco = new Core.Key.Xml.EnvCryptKey();
            aesKeyToXmlMapper.Map(key, xmlPoco);

            // Assert
            xmlPoco.Rsa.Should().NotBeNull().And.HaveCount(1);
            xmlPoco.Rsa[0].D.Should().Be("d");
            xmlPoco.Rsa[0].Dp.Should().Be("dp");
            xmlPoco.Rsa[0].Dq.Should().Be("dq");
            xmlPoco.Rsa[0].Exponent.Should().Be("exponent");
            xmlPoco.Rsa[0].Modulus.Should().Be("modulus");
            xmlPoco.Rsa[0].InverseQ.Should().Be("inverseQ");
            xmlPoco.Rsa[0].P.Should().Be("p");
            xmlPoco.Rsa[0].Q.Should().Be("q");

            converter.Verify(c => c.Encode(key.Key.D), Times.Once);
            converter.Verify(c => c.Encode(key.Key.DP), Times.Once);
            converter.Verify(c => c.Encode(key.Key.DQ), Times.Once);
            converter.Verify(c => c.Encode(key.Key.Exponent), Times.Once);
            converter.Verify(c => c.Encode(key.Key.Modulus), Times.Once);
            converter.Verify(c => c.Encode(key.Key.InverseQ), Times.Once);
            converter.Verify(c => c.Encode(key.Key.P), Times.Once);
            converter.Verify(c => c.Encode(key.Key.Q), Times.Once);
        }


        [Test]
        public void Given_ValidAESPublicKey_When_MapToXMLPOCO_Then_NameMappedCorrectly()
        {
            // Arrange
            var key = new RsaKey(new RSAParameters()
            {
                Exponent = new byte[1],
                Modulus = new byte[2],
            }, true);
            const string keyName = "My Rsa Key";
            key.Name = keyName;
            var converter = new Mock<IStringPersistConverter>(MockBehavior.Strict);
            converter.Setup(c => c.Encode(key.Key.Exponent)).Returns("exponent");
            converter.Setup(c => c.Encode(key.Key.Modulus)).Returns("modulus");

            // Act
            var aesKeyToXmlMapper = new RsaKeyToXmlMapper(converter.Object);
            var xmlPoco = new Core.Key.Xml.EnvCryptKey();
            aesKeyToXmlMapper.Map(key, xmlPoco);

            // Assert
            xmlPoco.Rsa.Should().NotBeNull().And.HaveCount(1);
            xmlPoco.Name.Should().Be(keyName);
            xmlPoco.Encryption.Should().Be(EnvCryptAlgoEnum.Rsa.ToString());
            xmlPoco.Type.Should().Be(AsymmetricKeyType.Public.ToString());
            xmlPoco.Rsa[0].Exponent.Should().Be("exponent");
            xmlPoco.Rsa[0].Modulus.Should().Be("modulus");
        }
    }
}