using System.Security.Cryptography;
using EnvCrypt.Core.EncryptionAlgo.Rsa.Key;
using EnvCrypt.Core.Key;
using EnvCrypt.Core.Utils;
using EnvCrypt.Core.Utils.IO;
using EnvCrypt.Core.Verb.GenerateKey.Persister;
using Moq;
using NUnit.Framework;
using EnvCryptKey = EnvCrypt.Core.Key.Xml.EnvCryptKey;

namespace EnvCrypt.Core.UnitTest.Verb.GenerateKey.Persister
{
    [TestFixture]
    public class RsaKeyFilePersisterTest
    {
        [Test]
        public void Given__When__Then_()
        {
            // Arrange
            var pocoMapper = new Mock<IKeyToExternalRepresentationMapper<RsaKey, EnvCryptKey>>();
            var serialisationUtil = new Mock<IXmlSerializationUtils<Core.Key.Xml.EnvCryptKey>>();
            var writer = new Mock<IStringToFileWriter>();

            var newKey = new RsaKeyGenerator().GetNewKey(new RsaKeyGenerationOptions(384, true));
            var opts = new AsymmetricKeyFilePersisterOptions();

            // Act
            var persister = new RsaKeyFilePersister(pocoMapper.Object, serialisationUtil.Object, writer.Object);
            persister.WriteToFile(newKey, opts);

            // Assert

        }
    }
}