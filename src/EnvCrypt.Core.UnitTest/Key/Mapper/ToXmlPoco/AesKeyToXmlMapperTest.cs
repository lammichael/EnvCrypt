using EnvCrypt.Core.Key.Aes;
using EnvCrypt.Core.Key.Mapper.Xml.ToXmlPoco;
using EnvCrypt.Core.Key.XmlPoco;
using EnvCrypt.Core.Utils;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace EnvCrypt.Core.UnitTest.Key.Mapper.ToXmlPoco
{
    [TestFixture]
    public class AesKeyToXmlMapperTest
    {
        [Test]
        public void Given_ValidAESKey_When_MapToXMLPOCO_Then_EncodingHelperClassMustBeused()
        {
            // Arrange
            var key = new AesKey()
            {
                Iv = RandomByteArrayUtils.CreateRandomByteArray(512),
                Key = RandomByteArrayUtils.CreateRandomByteArray(1024),
            };
            var ivMapsTo = "myIVString";
            var keyMapsTo = "myIVString";
            var converter = new Mock<IStringPersistConverter>(MockBehavior.Strict);
            converter.Setup(c => c.Encode(key.Iv)).Returns(ivMapsTo);
            converter.Setup(c => c.Encode(key.Key)).Returns(keyMapsTo);

            // Act
            var aesKeyToXmlMapper = new AesKeyToXmlMapper(converter.Object);
            var xmlPoco = new EnvCryptKey();
            aesKeyToXmlMapper.Map(key, xmlPoco);

            // Assert
            xmlPoco.Aes.Should().NotBeNull().And.HaveCount(1);
            xmlPoco.Aes[0].Iv.Should().Be(ivMapsTo);
            xmlPoco.Aes[0].Key.Should().Be(keyMapsTo);
            converter.Verify(c => c.Encode(key.Iv), Times.Once);
            converter.Verify(c => c.Encode(key.Key), Times.Once);
        }


        [Test]
        public void Given_ValidAESKey_When_MapToXMLPOCO_Then_NameMappedCorrectly()
        {
            // Arrange
            var keyName = "MyAESKey";
            var key = new AesKey()
            {
                Name = keyName,
                Iv = new byte[1],
                Key = new byte[1],
            };
            var converter = new Mock<IStringPersistConverter>();
            converter.Setup(c => c.Encode(It.IsAny<byte[]>())).Returns("a string not important to the test, but required to satisfy the method's post condition");

            // Act
            var aesKeyToXmlMapper = new AesKeyToXmlMapper(converter.Object);
            var xmlPoco = new EnvCryptKey();
            aesKeyToXmlMapper.Map(key, xmlPoco);

            // Assert
            xmlPoco.Aes.Should().NotBeNull().And.HaveCount(1);
            xmlPoco.Name.Should().Be(keyName);
        }
    }
}