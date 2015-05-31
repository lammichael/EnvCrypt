using System.Diagnostics;
using System.Security.Cryptography;
using EnvCrypt.Core.Key.Rsa;
using FluentAssertions;
using NUnit.Framework;

namespace EnvCrypt.Core.UnitTest.EncryptionAlgo.Rsa.Key
{
    [TestFixture]
    public class RsaKeyTest
    {
        [Test]
        public void Given_ValidRSAKey_When_PrivateKeyParametersChanged_Then_HashCodeIsTheSame()
        {
            // Arrange
            var key1 = new RsaKey(new RSAParameters()
            {
                Exponent = RandomByteArrayUtils.CreateRandomByteArray(5),
                Modulus = RandomByteArrayUtils.CreateRandomByteArray(5),
            }, true);
            var key2 = new RsaKey(new RSAParameters()
            {
                Exponent = key1.Key.Exponent,
                Modulus = key1.Key.Modulus,
                D = RandomByteArrayUtils.CreateRandomByteArray(5),
                DP = RandomByteArrayUtils.CreateRandomByteArray(5),
                DQ = RandomByteArrayUtils.CreateRandomByteArray(5),
                InverseQ = RandomByteArrayUtils.CreateRandomByteArray(5),
                P = RandomByteArrayUtils.CreateRandomByteArray(5),
                Q = RandomByteArrayUtils.CreateRandomByteArray(5),
            }, true);

            // Act
            // Assert
            key1.GetHashCode().Should().Be(key2.GetHashCode(), "hash code only takes public key parameters into account");
        }


        [Test]
        public void Given_ValidRSAKey_When_PublicKeyParametersChanged_Then_HashCodeIsTheSame()
        {
            // Arrange
            var key1 = new RsaKey(new RSAParameters()
            {
                Exponent = RandomByteArrayUtils.CreateRandomByteArray(5),
                Modulus = RandomByteArrayUtils.CreateRandomByteArray(5),
            }, true);
            var key2 = new RsaKey(new RSAParameters()
            {
                Exponent = key1.Key.Exponent,
                Modulus = RandomByteArrayUtils.CreateRandomByteArray(5),
            }, true);

            // Act
            // Assert
            key1.GetHashCode().Should().NotBe(key2.GetHashCode());
        }


        [Test]
        public void Given_ValidRSAKey_When_DifferentByteArraysInPublicKey_Then_HashCodeIsTheSame()
        {
            // Arrange
            var key1 = new RsaKey(new RSAParameters()
            {
                Exponent = new byte[] {1, 2},
                Modulus = new byte[] {1, 2, 3, 4, 5},
            }, true);
            var key2 = new RsaKey(new RSAParameters()
            {
                Exponent = new byte[] {1, 2},
                Modulus = new byte[] {1, 2, 3, 4, 5},
            }, true);

            // Act
            // Assert
            Debug.WriteLine("key1 hash: {0}", key1.GetHashCode());
            Debug.WriteLine("key2 hash: {0}", key2.GetHashCode());
            key1.GetHashCode().Should().Be(key2.GetHashCode());
        }
    }
}