using EnvCrypt.Core.EncryptionAlgo.Rsa;
using FluentAssertions;
using NUnit.Framework;

namespace EnvCrypt.Core.UnitTest.EncryptionAlgo.Rsa
{
    [TestFixture]
    class RsaKeyGeneratorTest
    {
        [Test]
        public void When_GetNewKey_Then_PrivateAndPublicKeyReturned()
        {
            // Arrange
            var generator = new RsaKeyGenerator();

            // Act
            var newKey = generator.GetNewKey();

            // Assert
            newKey.PrivateKey.Should().NotBeNull();
            newKey.PublicKey.Should().NotBeNull();
        }
    }
}
