using System.Security.Cryptography;
using EnvCrypt.Core.Key.Mapper;
using EnvCrypt.Core.Key.Rsa;
using EnvCrypt.Core.Key.XmlPoco;
using EnvCrypt.Core.Utils;
using EnvCrypt.Core.Utils.IO;
using EnvCrypt.Core.Verb.LoadKey;
using EnvCrypt.Core.Verb.LoadKey.Rsa;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace EnvCrypt.Core.UnitTest.Verb.LoadKey
{
    [TestFixture]
    public class RsaKeyLoaderTest
    {
        [Test]
        public void Given_ValidKeyPathAndNonEmptyKeyFile_When_Run_Then_KeyFileDeserialised()
        {
            // Arrange
            const string keyfile = @"C:\some\path\keyfile.eckey";
            const string keyFileContents = @"<?xml version=...";
            var xmlPoco = new EnvCryptKey();
            var rsaKey = new RsaKey(new RSAParameters(), true)
            {
                Name = "key"
            };

            var myFileMock = new Mock<IMyFile>(MockBehavior.Strict);
            myFileMock.Setup(f => f.Exists(keyfile)).Returns(true);

            var textReaderMock = new Mock<ITextReader>(MockBehavior.Strict);
            textReaderMock.Setup(r => r.ReadAllText(keyfile))
                .Returns(keyFileContents);

            var serialisationMock = new Mock<IXmlSerializationUtils<EnvCryptKey>>(MockBehavior.Strict);
            serialisationMock.Setup(u => u.Deserialize(keyFileContents))
                .Returns(xmlPoco);

            var mapperMock = new Mock<IExternalRepresentationToKeyMapper<EnvCryptKey, RsaKey>>(MockBehavior.Strict);
            mapperMock.Setup(m => m.Map(xmlPoco)).Returns(rsaKey);

            // Act
            var workflow = new RsaKeyFromXmlFileLoader(myFileMock.Object, textReaderMock.Object, serialisationMock.Object, mapperMock.Object);
            var keyPoco = workflow.Load(new KeyFromFileDetails() { FilePath = keyfile });

            // Assert
            keyPoco.Should().Be(rsaKey);
        }
    }
}