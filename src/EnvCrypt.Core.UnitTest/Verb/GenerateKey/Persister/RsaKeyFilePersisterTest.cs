using System.Text;
using EnvCrypt.Core.Key;
using EnvCrypt.Core.Key.Mapper;
using EnvCrypt.Core.Key.Rsa;
using EnvCrypt.Core.Utils;
using EnvCrypt.Core.Utils.IO;
using EnvCrypt.Core.Verb.GenerateKey.Persister;
using Moq;
using NUnit.Framework;
using EnvCryptKey = EnvCrypt.Core.Key.XmlPoco.EnvCryptKey;

namespace EnvCrypt.Core.UnitTest.Verb.GenerateKey.Persister
{
    [TestFixture]
    public class RsaKeyFilePersisterTest
    {
        [Test]
        public void Given_ValidKey_When_WriteToFile_Then_PublicAndPrivateKeyEncodedAndWrittenToFile()
        {
            // Arrange
            var privateKey = new RsaKeyGenerator().GetNewKey(new RsaKeyGenerationOptions()
            {
                KeySize = 384,
                UseOaepPadding = true,
                NewKeyName = "test"
            });

            var pocoMapper = new Mock<IKeyToExternalRepresentationMapper<RsaKey, EnvCryptKey>>();
            pocoMapper.Setup(m => m.Map(It.IsAny<RsaKey>(), It.IsAny<EnvCryptKey>())).Callback<RsaKey, EnvCryptKey>((key, keyXml) => keyXml.Type = key.Key.D == null ? KeyTypeEnum.Public.ToString() : keyXml.Type = KeyTypeEnum.Private.ToString());

            const string privateKeyXmlContents = @"<EnvCryptKey Type=""Private"" ... > ...";
            const string publicKeyXmlContents = @"<EnvCryptKey Type=""Public"" ... > ...";
            var usedEncoding = Encoding.UTF32;
            var serialisationUtil = new Mock<IXmlSerializationUtils<EnvCryptKey>>();
            serialisationUtil.Setup(u => u.Serialize(It.Is<EnvCryptKey>(p => p.Type == KeyTypeEnum.Private.ToString()))).Returns(privateKeyXmlContents);
            serialisationUtil.Setup(u => u.Serialize(It.Is<EnvCryptKey>(p => p.Type == KeyTypeEnum.Public.ToString()))).Returns(publicKeyXmlContents);
            serialisationUtil.Setup(u => u.GetUsedEncoding()).Returns(usedEncoding);
            var writer = new Mock<IStringToFileWriter>();

            
            const string privateKeyFile = @"C:\some\path\privatekey.xml";
            const string publicKeyFile = @"C:\some\path\publickey.xml";
            var opts = new AsymmetricKeyFilePersisterOptions()
            {
                NewPrivateKeyFullFilePath = privateKeyFile,
                NewPublicKeyFullFilePath = publicKeyFile,
                OverwriteFileIfExists = true
            };

            // Act
            var persister = new RsaKeyFilePersister(pocoMapper.Object, serialisationUtil.Object, writer.Object);
            persister.WriteToFile(privateKey, opts);

            // Assert
            serialisationUtil.Verify(u => u.Serialize(It.Is<EnvCryptKey>(p => p.Type == KeyTypeEnum.Private.ToString())), Times.Once);
            writer.Verify(w => w.Write(privateKeyFile, privateKeyXmlContents, true, usedEncoding), Times.Once);

            serialisationUtil.Verify(u => u.Serialize(It.Is<EnvCryptKey>(p => p.Type == KeyTypeEnum.Public.ToString())), Times.Once);
            writer.Verify(w => w.Write(publicKeyFile, publicKeyXmlContents, true, usedEncoding), Times.Once);
        }
    }
}