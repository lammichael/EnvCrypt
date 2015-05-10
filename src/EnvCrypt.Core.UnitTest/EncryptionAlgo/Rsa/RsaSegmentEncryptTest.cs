using System;
using System.Security.Cryptography;
using EnvCrypt.Core.EncryptionAlgo;
using EnvCrypt.Core.EncryptionAlgo.Rsa;
using EnvCrypt.Core.EncryptionAlgo.Rsa.Key;
using EnvCrypt.Core.EncryptionAlgo.Rsa.Utils;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace EnvCrypt.Core.UnitTest.EncryptionAlgo.Rsa
{
    [TestFixture]
    public class RsaSegmentEncryptTest
    {
        [Test]
        public void Given_BinaryData_When_Encrypt_Then_BinaryDataMustBeSplitUpAppropriately(
            [Values(10,20,30)] int lengthOfByteArray, [Values(9,10,11,19,20,21,29,30,31)] int maxSegmentSize)
        {
            // Arrange
            //      # of segments created
            var expectedArraysInList = (int) Math.Ceiling((double)lengthOfByteArray/maxSegmentSize);
            var toEncrypt = CreateRandomByteArray(lengthOfByteArray);

            var rsaKey = new RsaKey(new RSAParameters(), true);

            var algoMock = new Mock<IEncryptionAlgo<RsaKey>>();
            var maxEncryptionSizeCalcMock = new Mock<IRsaMaxEncryptionCalc>();
            maxEncryptionSizeCalcMock.Setup(calc => calc.GetMaxBytesThatCanBeEncrypted(rsaKey))
                .Returns(maxSegmentSize);
            var encrypter = new RsaSegmentEncrypt(algoMock.Object, maxEncryptionSizeCalcMock.Object);

            // Act
            var encryptedList = encrypter.Encrypt(toEncrypt, rsaKey);

            // Assert
            encryptedList.Should().HaveCount(expectedArraysInList);
        }


        private byte[] CreateRandomByteArray(int ofSize)
        {
            var ret = new byte[ofSize];
            var ran = new Random();
            ran.NextBytes(ret);
            return ret;
        }
    }
}