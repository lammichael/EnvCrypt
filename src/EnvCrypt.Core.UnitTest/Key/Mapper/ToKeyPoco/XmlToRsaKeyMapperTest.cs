using EnvCrypt.Core.Key.Mapper.Xml.ToKeyPoco;
using EnvCrypt.Core.Key.XmlPoco;
using EnvCrypt.Core.Utils;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace EnvCrypt.Core.UnitTest.Key.Mapper.ToKeyPoco
{
    [TestFixture]
    public class XmlToRsaKeyMapperTest
    {
        [Test]
        public void Given_ValidRSAKey_When_Mapped_Then_ValuesMappedCorrectly()
        {
            // Arrange
            const string keyName = "My Test Key";
            var xmlPoco = new EnvCryptKey()
            {
                Name = keyName,
                Rsa = new[]
                {
                    new EnvCryptKeyRsa()
                    {
                        D = "D",
                        Dp = "DP",
                        Exponent = "Exponent",
                        Dq = "DQ",
                        InverseQ = "InverseQ",
                        Modulus = "Modulus",
                        OaepPadding = true,
                        P = "P",
                        Q = "Q"
                    }
                },
                Aes = null,
            };

            var strConverterMock = new Mock<IStringPersistConverter>(MockBehavior.Strict);
            {
                var counter = 1;
                strConverterMock.Setup(c => c.Decode("D")).Returns(new byte[counter++]);
                strConverterMock.Setup(c => c.Decode("DP")).Returns(new byte[counter++]);
                strConverterMock.Setup(c => c.Decode("Exponent")).Returns(new byte[counter++]);
                strConverterMock.Setup(c => c.Decode("DQ")).Returns(new byte[counter++]);
                strConverterMock.Setup(c => c.Decode("InverseQ")).Returns(new byte[counter++]);
                strConverterMock.Setup(c => c.Decode("Modulus")).Returns(new byte[counter++]);
                strConverterMock.Setup(c => c.Decode("P")).Returns(new byte[counter++]);
                strConverterMock.Setup(c => c.Decode("Q")).Returns(new byte[counter++]);
            }
            // Act
            var mapper = new XmlToRsaKeyMapper(strConverterMock.Object);
            var res = mapper.Map(xmlPoco);

            // Assert
            {
                var counter = 1;
                res.UseOaepPadding.Should().BeTrue();
                res.Key.D.Should().BeEquivalentTo(new byte[counter++]);
                res.Key.DP.Should().BeEquivalentTo(new byte[counter++]);
                res.Key.Exponent.Should().BeEquivalentTo(new byte[counter++]);
                res.Key.DQ.Should().BeEquivalentTo(new byte[counter++]);
                res.Key.InverseQ.Should().BeEquivalentTo(new byte[counter++]);
                res.Key.Modulus.Should().BeEquivalentTo(new byte[counter++]);
                res.Key.P.Should().BeEquivalentTo(new byte[counter++]);
                res.Key.Q.Should().BeEquivalentTo(new byte[counter++]);
            }
        }


        [Test]
        public void Given_RSAPublicKey_When_Mapped_Then_NoException()
        {
            // Arrange
            var xmlPoco = new EnvCryptKey()
            {
                Name = "My Test Key",
                Rsa = new[]
                {
                    new EnvCryptKeyRsa()
                    {
                        Exponent = "Exponent",
                        Modulus = "Modulus",
                    }
                },
                Aes = null,
            };

            var strConverterMock = new Mock<IStringPersistConverter>(MockBehavior.Strict);
            {
                var counter = 1;
                strConverterMock.Setup(c => c.Decode("Exponent")).Returns(new byte[counter++]);
                strConverterMock.Setup(c => c.Decode("Modulus")).Returns(new byte[counter++]);
            }
            // Act
            var mapper = new XmlToRsaKeyMapper(strConverterMock.Object);
            var res = mapper.Map(xmlPoco);

            // Assert
            {
                var counter = 1;
                res.Key.Exponent.Should().BeEquivalentTo(new byte[counter++]);
                res.Key.Modulus.Should().BeEquivalentTo(new byte[counter++]);
            }
        }
    }
}