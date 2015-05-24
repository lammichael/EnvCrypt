using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using EnvCrypt.Core.EncryptionAlgo;
using EnvCrypt.Core.EncryptionAlgo.Rsa.Key;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace EnvCrypt.Core.UnitTest.EncryptionAlgo
{
    [TestFixture]
    public class SegmentEncryptionAlgoTest
    {
        [Test]
        public void Given_ArrayOfByteArrays_When_Decrypted_Then_ByteArrayConcatenated()
        {
            // Arrange
            var listOfByteArraysToDecrypt = new List<byte[]>()
            {
                new byte[] {1},
                new byte[] {2},
                new byte[] {3},
            };
            var decryptedBytes = new List<byte[]>()
            {
                new byte[] {1, 2, 3, 4},
                new byte[] {6, 7, 8},
                new byte[] {9, 10},
            };

            var someKey = new RsaKey(new RSAParameters(), true);
            var encryptionAlgoMock = new Mock<IEncryptionAlgo<RsaKey>>(MockBehavior.Strict);
            encryptionAlgoMock.Setup(a => a.Decrypt(listOfByteArraysToDecrypt[0], someKey))
                .Returns(decryptedBytes[0]);
            encryptionAlgoMock.Setup(a => a.Decrypt(listOfByteArraysToDecrypt[1], someKey))
                .Returns(decryptedBytes[1]);
            encryptionAlgoMock.Setup(a => a.Decrypt(listOfByteArraysToDecrypt[2], someKey))
                .Returns(decryptedBytes[2]);

            // Act
            var segEncrypt = new SegmentEncryptionAlgoStub(encryptionAlgoMock.Object);
            var result = segEncrypt.Decrypt(listOfByteArraysToDecrypt, someKey);

            // Assert
            result.Should().BeEquivalentTo(
                decryptedBytes[0].Concat(decryptedBytes[1]).Concat(decryptedBytes[2]));
        }


        /// <summary>
        /// Only used to test the functionality in the abstract class.
        /// </summary>
        private class SegmentEncryptionAlgoStub : SegmentEncryptionAlgo<RsaKey>
        {
            public SegmentEncryptionAlgoStub(IEncryptionAlgo<RsaKey> encryptionAlgo) : base(encryptionAlgo)
            {
            }

            public override IList<byte[]> Encrypt(byte[] binaryData, RsaKey usingKey)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}