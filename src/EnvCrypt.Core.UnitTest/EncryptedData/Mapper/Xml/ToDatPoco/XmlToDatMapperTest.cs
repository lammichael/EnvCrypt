using System.Collections.Generic;
using EnvCrypt.Core.EncrypedData.Mapper.Xml.ToDatPoco;
using EnvCrypt.Core.EncrypedData.XmlPoco;
using EnvCrypt.Core.EncryptionAlgo;
using EnvCrypt.Core.Utils;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace EnvCrypt.Core.UnitTest.EncryptedData.Mapper.Xml.ToDatPoco
{
    [TestFixture]
    public class XmlToDatMapperTest
    {
        [Test]
        public void Given_ValidXMLPOCO_When_Map_Then_ValuesCorrectlyMapped()
        {
            // Arrange
            var xmlProdKey = new EnvCryptEncryptedDataCategoryEntryDecryption()
            {
                KeyName = "Production Key",
                Algo = "Rsa",
                Hash = 123,
            };

            var xmlPoco = new EnvCryptEncryptedData()
            {
                Items = new[]
                {
                    new EnvCryptEncryptedDataCategory()
                    {
                        Name = "Production",
                        Entry = new[]
                        {
                            new EnvCryptEncryptedDataCategoryEntry()
                            {
                                Name = "root password",
                                Decryption = xmlProdKey,
                                EncryptedValue = new[]
                                {
                                    new EnvCryptEncryptedDataCategoryEntryEncryptedValue()
                                    {
                                        Value = "Beep"
                                    }
                                }
                            },
                            new EnvCryptEncryptedDataCategoryEntry()
                            {
                                Name = "database password",
                                Decryption = xmlProdKey,
                                EncryptedValue = new[]
                                {
                                    new EnvCryptEncryptedDataCategoryEntryEncryptedValue()
                                    {
                                        Value = "Boop"
                                    }
                                }
                            }
                        }
                    },
                    new EnvCryptEncryptedDataCategory()
                    {
                        Name = "UAT",
                        Entry = new[]
                        {
                            new EnvCryptEncryptedDataCategoryEntry()
                            {
                                Name = "logon password",
                                Decryption = null,
                                EncryptedValue = new[]
                                {
                                    new EnvCryptEncryptedDataCategoryEntryEncryptedValue()
                                    {
                                        Value = "Bab"
                                    },
                                    new EnvCryptEncryptedDataCategoryEntryEncryptedValue()
                                    {
                                        Value = "OrBob"
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var strConverterMock = new Mock<IEncryptedDetailsPersistConverter>(MockBehavior.Strict);
            strConverterMock.Setup(c => c.Decode(It.IsAny<string>(), It.IsAny<EnvCryptAlgoEnum>()))
                .Returns<string>(s => new byte[s.Length]);

            // Act
            var mapper = new XmlToDatMapper(strConverterMock.Object);
            var res = mapper.Map(xmlPoco);

            // Assert
            res.Categories.Should().NotBeNull();
            res.Categories.Should().HaveCount(2, "Prod & UAT categories are defined");
            res.Categories[0].Name.Should().Be("Production");
            res.Categories[1].Name.Should().Be("UAT");
            res.Categories[0].Entries.Should().HaveCount(2);
            res.Categories[0].Entries[0].Name.Should().Be("root password");
            res.Categories[0].Entries[0].EncryptionAlgorithm.Should().Be(EnvCryptAlgoEnum.Rsa);
            res.Categories[0].Entries[0].KeyHash.Should().Be(xmlProdKey.Hash);
            res.Categories[0].Entries[0].EncryptedValue.Should().HaveCount(1);
            res.Categories[0].Entries[0].EncryptedValue[0].Should().BeEquivalentTo(
                new byte[4]);
            res.Categories[1].Entries.Should().HaveCount(1);
            res.Categories[1].Entries[0].EncryptionAlgorithm.Should().Be(EnvCryptAlgoEnum.PlainText, "there is no decryption in the XML");
            res.Categories[1].Entries[0].EncryptedValue.Should().HaveCount(2);
            res.Categories[1].Entries[0].EncryptedValue[0].Should().BeEquivalentTo(
                new byte[3]);
        }


        [Test]
        public void Given_XMLWithPlainTextAndRSA_When_Map_Then_CorrectAlgoEnumPassedIntoConverter()
        {
            // Arrange
            // Act
            // Assert
            Assert.Fail();
        }
    }
}