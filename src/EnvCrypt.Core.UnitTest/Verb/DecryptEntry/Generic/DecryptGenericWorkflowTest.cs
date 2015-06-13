using System;
using System.Collections.Generic;
using EnvCrypt.Core.EncrypedData.Poco;
using EnvCrypt.Core.EncryptionAlgo;
using EnvCrypt.Core.Key.Aes;
using EnvCrypt.Core.Verb.DecryptEntry;
using EnvCrypt.Core.Verb.DecryptEntry.Aes;
using EnvCrypt.Core.Verb.DecryptEntry.Generic;
using EnvCrypt.Core.Verb.DecryptEntry.PlainText;
using EnvCrypt.Core.Verb.DecryptEntry.Rsa;
using EnvCrypt.Core.Verb.LoadDat;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace EnvCrypt.Core.UnitTest.Verb.DecryptEntry.Generic
{
    [TestFixture]
    public class DecryptGenericWorkflowTest
    {
        [Test]
        public void Given_TwoDifferentEncAlgosRequestedToBeDecrypted_When_Run_Then_ExceptionThrown()
        {
            // Arrange
            Mock<IDatLoader<DatFromFileLoaderOptions>> datLoaderMock;
            var plainTextEntryWorkflowBuilderMock = new Mock<IDecryptPlainTextEntryWorkflowBuilder>(MockBehavior.Strict);
            var rsaEntryWorkflowBuilderMock = new Mock<IDecryptRsaEntryWorkflowBuilder>(MockBehavior.Strict);
            var aesEntryWorkflowBuilderMock = new Mock<IDecryptAesEntryWorkflowBuilder>(MockBehavior.Strict);
            var options = TwoDifferentAlgosExceptionTestSetup(out datLoaderMock);

            

            // Act
            var workflow = new DecryptGenericWorkflow(datLoaderMock.Object, plainTextEntryWorkflowBuilderMock.Object, rsaEntryWorkflowBuilderMock.Object, aesEntryWorkflowBuilderMock.Object);
            Action act = () => workflow.Run(options);

            // Assert
            act.ShouldThrow<EnvCryptException>();
        }


        [Test]
        public void Given_TwoDifferentEncAlgos_When_NotRequestedToBeEncrypted_Then_ExceptionNotThrown()
        {
            // Arrange
            Mock<IDatLoader<DatFromFileLoaderOptions>> datLoaderMock;
            var plainTextEntryWorkflowBuilderMock = new Mock<IDecryptPlainTextEntryWorkflowBuilder>(MockBehavior.Strict);
            var rsaEntryWorkflowBuilderMock = new Mock<IDecryptRsaEntryWorkflowBuilder>(MockBehavior.Strict);
            var aesEntryWorkflowBuilderMock = new Mock<IDecryptAesEntryWorkflowBuilder>(MockBehavior.Strict);
            aesEntryWorkflowBuilderMock.Setup(
                b => b.WithDatLoader(It.IsAny<IDatLoader<DatFromFileLoaderOptions>>()))
                .Returns(aesEntryWorkflowBuilderMock.Object);
            aesEntryWorkflowBuilderMock.Setup(
                b => b.Run(It.IsAny<DecryptEntryWorkflowOptions>()))
                .Returns(new List<EntriesDecrypterResult<AesKey>>());
            var options = TwoDifferentAlgosExceptionTestSetup(out datLoaderMock);

            options.CategoryEntryPair.RemoveAt(0);

            // Act
            var workflow = new DecryptGenericWorkflow(datLoaderMock.Object, plainTextEntryWorkflowBuilderMock.Object, rsaEntryWorkflowBuilderMock.Object, aesEntryWorkflowBuilderMock.Object);
            Action act = () => workflow.Run(options);

            // Assert
            act.ShouldNotThrow<EnvCryptException>();
        }


        private static DecryptGenericWorkflowOptions TwoDifferentAlgosExceptionTestSetup(out Mock<IDatLoader<DatFromFileLoaderOptions>> datLoaderMock)
        {
            var options = new DecryptGenericWorkflowOptions()
            {
                DatFilePath = @"X:\tmp\generic.dat",
                CategoryEntryPair = new List<CategoryEntryPair>()
                {
                    new CategoryEntryPair("prod", "password"),
                    new CategoryEntryPair("uat", "password"),
                }
            };

            var dat = new EnvCryptDat()
            {
                Categories = new[]
                {
                    new Category()
                    {
                        Name = options.CategoryEntryPair[0].Category,
                        Entries = new[]
                        {
                            new Entry()
                            {
                                Name = options.CategoryEntryPair[0].Entry,
                                EncryptionAlgorithm = EnvCryptAlgoEnum.Rsa
                            }
                        }
                    },
                    new Category()
                    {
                        Name = options.CategoryEntryPair[1].Category,
                        Entries = new[]
                        {
                            new Entry()
                            {
                                Name = options.CategoryEntryPair[1].Entry,
                                EncryptionAlgorithm = EnvCryptAlgoEnum.Aes
                            }
                        }
                    }
                }
            };


            datLoaderMock = new Mock<IDatLoader<DatFromFileLoaderOptions>>(MockBehavior.Strict);
            datLoaderMock.Setup(l => l.Load(It.Is<DatFromFileLoaderOptions>(o => o.DatFilePath == options.DatFilePath)))
                .Returns(dat);

            return options;
        }
    }
}