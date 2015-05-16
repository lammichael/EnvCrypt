using System;
using System.Linq;
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
    public class RsaSegmentEncrypterTest
    {
        [Test]
        public void Given_BinaryData_When_Encrypt_Then_BinaryDataMustBeSplitUpAppropriately(
            [Values(10,20,30)] int lengthOfByteArray, 
            [Values(9,10,11,19,20,21,29,30,31)] int maxSegmentSize)
        {
            // Arrange
            byte[] toEncrypt;
            RsaKey rsaKey;
            Mock<IEncryptionAlgo<RsaKey>> algoMock;
            RsaSegmentEncrypter encrypter;
            var expectedArraysInList = SetupMethod(lengthOfByteArray, maxSegmentSize, out toEncrypt, out rsaKey, out algoMock, out encrypter);

            // Act
            var encryptedList = encrypter.Encrypt(toEncrypt, rsaKey);

            // Assert
            encryptedList.Should().HaveCount(expectedArraysInList);
            //      Each segment should be empty because the mocked algo returns nothing
            foreach (var encryptedSegment in encryptedList)
            {
                encryptedSegment.Should().BeEquivalentTo(new byte[0]);
            }
        }


        [Test]
        public void Given_BinaryData_When_Encrypt_Then_AlgoMustBeUsedToEncryptBinaryDataInSegments(
            [Values(10, 20, 30)] int lengthOfByteArray, 
            [Values(9, 10, 11, 19, 20, 21, 29, 30, 31)] int maxSegmentSize)
        {
            // Arrange
            byte[] toEncrypt;
            RsaKey rsaKey;
            Mock<IEncryptionAlgo<RsaKey>> algoMock;
            RsaSegmentEncrypter encrypter;
            var expectedArraysInList = SetupMethod(lengthOfByteArray, maxSegmentSize, out toEncrypt, out rsaKey, out algoMock, out encrypter);

            // Act
            encrypter.Encrypt(toEncrypt, rsaKey);

            // Assert
            algoMock.Verify(a => a.Encrypt(It.IsAny<byte[]>(), rsaKey), 
                Times.Exactly(expectedArraysInList));
            //      Check that the mock was called with the correct byte arrays
            for (int arrayI = 0; arrayI < expectedArraysInList; arrayI++)
            {
                //      Get start and end index for the current block
                var startI = maxSegmentSize*arrayI;
                var endI = Math.Min(startI + maxSegmentSize, lengthOfByteArray);
                var lengthOfCurrentArray = endI - startI;
                if (lengthOfCurrentArray <= 0)
                {
                    Assert.Fail("length of array that was encrypted cannot be <= 0");
                }
                var segmentEncrypted = new byte[lengthOfCurrentArray];
                Buffer.BlockCopy(toEncrypt, startI, 
                    segmentEncrypted, 0, 
                    lengthOfCurrentArray);

                algoMock.Verify(a => a.Encrypt(It.Is<byte[]>(b => b.SequenceEqual(segmentEncrypted)), rsaKey),
                Times.Once);
            }
        }


        private int SetupMethod(int lengthOfByteArray, int maxSegmentSize, out byte[] toEncrypt, out RsaKey rsaKey,
            out Mock<IEncryptionAlgo<RsaKey>> algoMock, out RsaSegmentEncrypter encrypter)
        {
            //      # of segments created
            var expectedArraysInList = (int) Math.Ceiling((double) lengthOfByteArray/maxSegmentSize);
            toEncrypt = RandomByteArrayUtils.CreateRandomByteArray(lengthOfByteArray);

            rsaKey = new RsaKey(new RSAParameters(), true);
            var rsaKeyCopy = rsaKey;

            algoMock = new Mock<IEncryptionAlgo<RsaKey>>();
            var maxEncryptionSizeCalcMock = new Mock<IRsaMaxEncryptionCalc>();
            maxEncryptionSizeCalcMock.Setup(calc => calc.GetMaxBytesThatCanBeEncrypted(rsaKeyCopy))
                .Returns(maxSegmentSize);
            encrypter = new RsaSegmentEncrypter(algoMock.Object, maxEncryptionSizeCalcMock.Object);
            return expectedArraysInList;
        }
    }
}