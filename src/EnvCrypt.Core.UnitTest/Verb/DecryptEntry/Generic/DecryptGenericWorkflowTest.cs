using System;
using System.Collections.Generic;
using System.Linq;
using EnvCrypt.Core.EncrypedData.Poco;
using EnvCrypt.Core.EncryptionAlgo;
using EnvCrypt.Core.Key.Aes;
using EnvCrypt.Core.Key.PlainText;
using EnvCrypt.Core.Key.Rsa;
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


        [Test]
        public void Given_PlainTextDecryptionRequest_When_Run_Then_PlainTextWorkflowCalled()
        {
            // Arrange
            Mock<IDatLoader<DatFromFileLoaderOptions>> datLoaderMock;

            var rsaEntryWorkflowBuilderMock = new Mock<IDecryptRsaEntryWorkflowBuilder>(MockBehavior.Strict);
            var aesEntryWorkflowBuilderMock = new Mock<IDecryptAesEntryWorkflowBuilder>(MockBehavior.Strict);

            var plainTextEntryWorkflowBuilderMock = new Mock<IDecryptPlainTextEntryWorkflowBuilder>(MockBehavior.Strict);
            plainTextEntryWorkflowBuilderMock.Setup(
                b => b.WithDatLoader(It.IsAny<IDatLoader<DatFromFileLoaderOptions>>()))
                .Returns(plainTextEntryWorkflowBuilderMock.Object);
            plainTextEntryWorkflowBuilderMock.Setup(
                b => b.Build())
                .Returns(plainTextEntryWorkflowBuilderMock.Object);
            plainTextEntryWorkflowBuilderMock.Setup(
                b => b.Run(It.IsAny<DecryptPlainTextEntryWorkflowOptions>()))
                .Returns(new List<EntriesDecrypterResult<PlainTextKey>>());

            var options = TwoDifferentAlgosExceptionTestSetup(out datLoaderMock);

            options.CategoryEntryPair = options.CategoryEntryPair.Where(p => p.Category == "dev").ToList();

            // Act
            var workflow = new DecryptGenericWorkflow(datLoaderMock.Object, plainTextEntryWorkflowBuilderMock.Object, rsaEntryWorkflowBuilderMock.Object, aesEntryWorkflowBuilderMock.Object);
            var res = workflow.Run(options);

            // Assert
            plainTextEntryWorkflowBuilderMock.Verify(
                b => b.Run(It.IsAny<DecryptPlainTextEntryWorkflowOptions>()), 
                Times.Once);
        }


        [Test]
        public void Given_RsaDecryptionRequest_When_Run_Then_RsaWorkflowCalled()
        {
            // Arrange
            Mock<IDatLoader<DatFromFileLoaderOptions>> datLoaderMock;

            var rsaEntryWorkflowBuilderMock = new Mock<IDecryptRsaEntryWorkflowBuilder>(MockBehavior.Strict);
            var aesEntryWorkflowBuilderMock = new Mock<IDecryptAesEntryWorkflowBuilder>(MockBehavior.Strict);
            var plainTextEntryWorkflowBuilderMock = new Mock<IDecryptPlainTextEntryWorkflowBuilder>(MockBehavior.Strict);

            rsaEntryWorkflowBuilderMock.Setup(
                b => b.WithDatLoader(It.IsAny<IDatLoader<DatFromFileLoaderOptions>>()))
                .Returns(rsaEntryWorkflowBuilderMock.Object);
            rsaEntryWorkflowBuilderMock.Setup(
                b => b.Build())
                .Returns(rsaEntryWorkflowBuilderMock.Object);
            rsaEntryWorkflowBuilderMock.Setup(
                b => b.Run(It.IsAny<DecryptEntryWorkflowOptions>()))
                .Returns(new List<EntriesDecrypterResult<RsaKey>>());

            var options = TwoDifferentAlgosExceptionTestSetup(out datLoaderMock);

            options.CategoryEntryPair = options.CategoryEntryPair.Where(p => p.Category == "prod").ToList();

            // Act
            var workflow = new DecryptGenericWorkflow(
                datLoaderMock.Object,
                plainTextEntryWorkflowBuilderMock.Object,
                rsaEntryWorkflowBuilderMock.Object,
                aesEntryWorkflowBuilderMock.Object);
            workflow.Run(options);

            // Assert
            rsaEntryWorkflowBuilderMock.Verify(
                b => b.Run(It.IsAny<DecryptEntryWorkflowOptions>()),
                Times.Once);
        }


        [Test]
        public void Given_AesDecryptionRequest_When_Run_Then_AesWorkflowCalled()
        {
            // Arrange
            Mock<IDatLoader<DatFromFileLoaderOptions>> datLoaderMock;

            var rsaEntryWorkflowBuilderMock = new Mock<IDecryptRsaEntryWorkflowBuilder>(MockBehavior.Strict);
            var aesEntryWorkflowBuilderMock = new Mock<IDecryptAesEntryWorkflowBuilder>(MockBehavior.Strict);
            var plainTextEntryWorkflowBuilderMock = new Mock<IDecryptPlainTextEntryWorkflowBuilder>(MockBehavior.Strict);

            aesEntryWorkflowBuilderMock.Setup(
                b => b.WithDatLoader(It.IsAny<IDatLoader<DatFromFileLoaderOptions>>()))
                .Returns(aesEntryWorkflowBuilderMock.Object);
            aesEntryWorkflowBuilderMock.Setup(
                b => b.Build())
                .Returns(aesEntryWorkflowBuilderMock.Object);
            aesEntryWorkflowBuilderMock.Setup(
                b => b.Run(It.IsAny<DecryptEntryWorkflowOptions>()))
                .Returns(new List<EntriesDecrypterResult<AesKey>>());

            var options = TwoDifferentAlgosExceptionTestSetup(out datLoaderMock);

            options.CategoryEntryPair = options.CategoryEntryPair.Where(p => p.Category == "uat").ToList();

            // Act
            var workflow = new DecryptGenericWorkflow(
                datLoaderMock.Object,
                plainTextEntryWorkflowBuilderMock.Object,
                rsaEntryWorkflowBuilderMock.Object,
                aesEntryWorkflowBuilderMock.Object);
            workflow.Run(options);

            // Assert
            aesEntryWorkflowBuilderMock.Verify(
                b => b.Run(It.IsAny<DecryptEntryWorkflowOptions>()),
                Times.Once);
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
                    new CategoryEntryPair("dev", "username"),
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
                    },
                    new Category()
                    {
                        Name = options.CategoryEntryPair[2].Category,
                        Entries = new[]
                        {
                            new Entry()
                            {
                                Name = options.CategoryEntryPair[2].Entry,
                                EncryptionAlgorithm = EnvCryptAlgoEnum.PlainText
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