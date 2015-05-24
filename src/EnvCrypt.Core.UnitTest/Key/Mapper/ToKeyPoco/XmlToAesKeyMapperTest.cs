using EnvCrypt.Core.Key.Mapper.Xml.ToKeyPoco;
using EnvCrypt.Core.Key.XmlPoco;
using EnvCrypt.Core.Utils;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace EnvCrypt.Core.UnitTest.Key.Mapper.ToKeyPoco
{
    [TestFixture]
    public class XmlToAesKeyMapperTest
    {
        [Test]
        public void Given_ValidAESKey_When_Mapped_Then_ValuesMappedCorrectly()
        {
            // Arrange
            const string keyName = "My Test Key";
            var xmlPoco = new EnvCryptKey()
            {
                Name = keyName,
                Rsa = null,
                Aes = new []
                {
                    new EnvCryptKeyAes()
                    {
                        Iv = "Iv",
                        Key = "Key"
                    }
                },
            };

            var strConverterMock = new Mock<IStringPersistConverter>(MockBehavior.Strict);
            {
                var counter = 1;
                strConverterMock.Setup(c => c.Decode("Iv")).Returns(new byte[counter++]);
                strConverterMock.Setup(c => c.Decode("Key")).Returns(new byte[counter++]);
            }
            // Act
            var mapper = new XmlToAesKeyMapper(strConverterMock.Object);
            var res = mapper.Map(xmlPoco);

            // Assert
            {
                var counter = 1;
                res.Iv.Should().BeEquivalentTo(new byte[counter++]);
                res.Key.Should().BeEquivalentTo(new byte[counter++]);
            }
        }
    }
}