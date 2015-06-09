﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using EnvCrypt.Core.EncrypedData.Poco;
using EnvCrypt.Core.EncrypedData.UserStringConverter;
using EnvCrypt.Core.EncryptionAlgo;
using EnvCrypt.Core.Key.Aes;
using EnvCrypt.Core.Key.Rsa;
using EnvCrypt.Core.Utils;
using EnvCrypt.Core.Utils.IO;
using EnvCrypt.Core.Verb.DecryptEntry;
using EnvCrypt.Core.Verb.DecryptEntry.Aes;
using EnvCrypt.Core.Verb.DecryptEntry.Audit;
using EnvCrypt.Core.Verb.GenerateKey;
using EnvCrypt.Core.Verb.GenerateKey.Aes;
using EnvCrypt.Core.Verb.GenerateKey.Persister.Symmetric;
using EnvCrypt.Core.Verb.LoadDat;
using EnvCrypt.Core.Verb.LoadKey;
using EnvCrypt.Core.Verb.LoadKey.Aes;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace EnvCrypt.Core.UnitTest.Verb.DecryptEntry.Audit
{
    [TestFixture]
    public class ToFileAuditLoggerTest
    {
        [Test]
        public void Given__When__Then_()
        {
            // Arrange
            //      Generate random key
            const string keyName = "key to test audit";
            var aesKeyGen = new AesKeyGenerator();
            var aesKey = aesKeyGen.GetNewKey(new AesKeyGenerationOptions()
            {
                KeySize = GenerateAesKeyBuilder.DefaultAesKeySize,
                NewKeyName = keyName
            });
            var keyLoaderMock = new Mock<IKeyLoader<AesKey, KeyFromFileDetails>>(MockBehavior.Strict);
            keyLoaderMock.Setup(l => l.Load(It.IsAny<KeyFromFileDetails>()))
                .Returns(aesKey);

            IUserStringConverter converter = new Utf16LittleEndianUserStringConverter();
            const string categoryName = "my category";
            const string entryName = "my entry";
            var datPoco = new EnvCryptDat()
            {
                Categories = new[]
                {
                    new Category()
                    {
                        Name = categoryName,
                        Entries = new[]
                        {
                            new Entry()
                            {
                                Name = entryName,
                                EncryptedValue = new[] { new byte[1] }, // not important for this test
                                EncryptionAlgorithm = aesKey.Algorithm,
                                KeyName = aesKey.Name,
                                KeyHash = aesKey.GetHashCode()
                            }
                        }
                    }
                }
            };
            var datLoaderMock = new Mock<IDatLoader>();
            datLoaderMock.Setup(loader => loader.Load(It.IsAny<string>()))
                .Returns(datPoco);

            var encryptionAlgoMock = new Mock<ISegmentEncryptionAlgo<AesKey>>();
            encryptionAlgoMock.Setup(a => a.Decrypt(It.IsAny<IList<byte[]>>(), aesKey))
                .Returns(converter.Encode("not important for this test"));


            var utcNow = new DateTime(2015, 01, 01);
            var dateTimeMock = new Mock<IMyDateTime>();
            dateTimeMock.Setup(t => t.UtcNow()).Returns(utcNow);
            var fileInfoFactoryMock = new Mock<IMyFileInfoFactory>();
            var fileInfoMock = new Mock<IMyFileInfo>();
            fileInfoMock.Setup(f => f.CreationTimeUtc)
                .Returns(utcNow.AddDays(-2));
            fileInfoFactoryMock.Setup(f => f.GetNewInstance(It.IsRegex(@".+deleteme")))
                .Returns(fileInfoMock.Object);

            using (var tempDir = new TempDir())
            {
                var loggerConfig = new ToFileAuditLoggerConfig()
                {
                    LogDirectory = tempDir.TempDirectory,
                    NumberOfDaysSinceCreationToKeep = 1
                };
                var auditLogger = new ToFileAuditLogger<AesKey, DecryptEntryWorkflowOptions>(
                    loggerConfig, new MyDirectory(), new MyFile(),
                    dateTimeMock.Object, fileInfoFactoryMock.Object);


                // Act
                var deleteFile1Path = Path.Combine(tempDir.TempDirectory, ".deleteme");
                File.WriteAllText(deleteFile1Path, "somerandomtext");
                var deleteFile2Path = Path.Combine(tempDir.TempDirectory, "asd.deleteme");
                File.WriteAllText(deleteFile2Path, "somerandomtext2");
                var workflow =
                    new DecryptAesEntryWorkflowBuilder()
                        .WithKeyLoader(keyLoaderMock.Object)
                        .WithDatLoader(datLoaderMock.Object)
                        .WithAesSegmentEncryptionAlgo(encryptionAlgoMock.Object)
                        .WithAuditLogger(auditLogger)
                        .Build().Run(new DecryptEntryWorkflowOptions()
                        {
                            CategoryEntryPair = new[]
                            {
                                new CategoryEntryPair(categoryName, entryName)
                            },
                            DatFilePath = "null",
                            KeyFilePaths = new[]
                            {
                                "null"
                            }
                        });


                // Assert
                File.Exists(deleteFile1Path).Should().BeFalse();
                File.Exists(deleteFile2Path).Should().BeFalse();
                File.Exists(Path.Combine(tempDir.TempDirectory, string.Format(loggerConfig.FileNameFormat,
                    utcNow.ToString("O"), 
                Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName)) +
                           loggerConfig.LogFileExtension))
                           .Should().BeTrue();
            }
        }
    }
}