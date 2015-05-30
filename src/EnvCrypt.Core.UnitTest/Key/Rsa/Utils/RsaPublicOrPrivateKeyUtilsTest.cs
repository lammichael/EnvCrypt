using System;
using System.Security.Cryptography;
using EnvCrypt.Core.Key;
using EnvCrypt.Core.Key.Rsa;
using EnvCrypt.Core.Key.Rsa.Utils;
using FluentAssertions;
using NUnit.Framework;

namespace EnvCrypt.Core.UnitTest.Key.Rsa.Utils
{
    [TestFixture]
    public class RsaPublicOrPrivateKeyUtilsTest
    {
        [Test]
        public void Given_RsaKeyWithJustExponent_When_GetKeyType_Then_ExceptionThrown()
        {
            // Arrange
            var rsaKey = new RsaKey(new RSAParameters()
            {
                Exponent = new byte[1]
            }, true);

            // Act
            Action act = () => rsaKey.GetKeyType();

            // Assert
            act.ShouldThrow<EnvCryptException>().And.Message.Should().Contain("does not have enough data to be a public or private key");
        }


        [Test]
        public void Given_ValidPublicKey_When_GetKeyType_Then_ExceptionThrown()
        {
            // Arrange
            var rsaKey = new RsaKey(new RSAParameters()
            {
                Exponent = new byte[1]
            }, true);

            // Act
            Action act = () => rsaKey.GetKeyType();

            // Assert
            act.ShouldThrow<EnvCryptException>().And.Message.Should().Contain("does not have enough data to be a public or private key");
        }


        [Test]
        public void Given_PublicKeyWithDetailsFromPrivateKey_When_GetKeyType_Then_ExceptionThrownOrPublicEnumIsRetunedDependingOnFlag(
            [Values(true, false)] bool throwExceptionWhenPublicKeyHasTooMuchInfo)
        {
            // Arrange
            var rsaKey = new RsaKey(new RSAParameters()
            {
                Exponent = new byte[1],
                Modulus = new byte[1],
                D = new byte[1]
            }, true);

            // Act
            var res = KeyTypeEnum.Private;
            Action act = () => res = rsaKey.GetKeyType(throwExceptionWhenPublicKeyHasTooMuchInfo);

            // Assert
            if (throwExceptionWhenPublicKeyHasTooMuchInfo)
            {
                act.ShouldThrow<EnvCryptException>()
                    .And.Message.Should()
                    .Contain("has information from the private key");
            }
            else
            {
                act.ShouldNotThrow<EnvCryptException>();
                res.Should().Be(KeyTypeEnum.Public);
            }
        }


        [Test]
        public void Given_ValidPublicKey_When_GetKeyType_Then_CorrectEnumValueReturned()
        {
            // Arrange
            var rsaKey = new RsaKey(new RSAParameters()
            {
                Exponent = new byte[1],
                Modulus = new byte[1],
            }, true);

            // Act
            var result = rsaKey.GetKeyType(true);

            // Assert
            result.Should().Be(KeyTypeEnum.Public);
        }


        [Test]
        public void Given_ValidPrivateKey_When_GetKeyType_Then_CorrectEnumValueReturned()
        {
            // Arrange
            var rsaKey = new RsaKey(new RSAParameters()
            {
                D = new byte[1],
                DP = new byte[1],
                Exponent = new byte[1],
                Modulus = new byte[1],
                DQ = new byte[1],
                InverseQ = new byte[1],
                P = new byte[1],
                Q = new byte[1],
            }, true);

            // Act
            var result = rsaKey.GetKeyType();

            // Assert
            result.Should().Be(KeyTypeEnum.Private);
        }
    }
}