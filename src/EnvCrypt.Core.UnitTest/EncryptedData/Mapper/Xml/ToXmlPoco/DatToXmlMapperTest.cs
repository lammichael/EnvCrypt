using EnvCrypt.Core.EncrypedData.Mapper.Xml.ToXmlPoco;
using EnvCrypt.Core.EncrypedData.Poco;
using EnvCrypt.Core.EncrypedData.XmlPoco;
using EnvCrypt.Core.EncryptionAlgo;
using EnvCrypt.Core.Utils;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace EnvCrypt.Core.UnitTest.EncryptedData.Mapper.Xml.ToXmlPoco
{
    [TestFixture]
    public class DatToXmlMapperTest
    {
        [Test]
        public void Given_ValidPOCO_When_Mapped_Then_CorrectXMLPOCOCreated()
        {
            // Arrange
            var datPoco = new EnvCryptDat()
            {
                Categories = new[]
                {
                    new Category()
                    {
                        Name = "Production",
                        Entries = new[]
                        {
                            new Entry()
                            {
                                Name = "database URL",
                                EncryptionAlgorithm = EnvCryptAlgoEnum.PlainText,
                                EncryptedValue = new[]
                                {
                                    new byte[1],
                                }
                            },
                            new Entry()
                            {
                                Name = "root password",
                                EncryptionAlgorithm = EnvCryptAlgoEnum.Rsa,
                                KeyHash = 1,
                                KeyName = "prod key",
                                EncryptedValue = new[]
                                {
                                    new byte[2],
                                    new byte[3],
                                }
                            }
                        }
                    },
                    new Category()
                    {
                        Name = "UAT",
                        Entries = new[]
                        {
                            new Entry()
                            {
                                Name = "root password",
                                EncryptionAlgorithm = EnvCryptAlgoEnum.Aes,
                                KeyHash = 2,
                                KeyName = "uat key",
                                EncryptedValue = new[]
                                {
                                    new byte[4],
                                }
                            }
                        }
                    }
                }
            };

            var strConverterMock = new Mock<IEncryptedDetailsPersistConverter>();
            //      Array of length 1 returns "1", length 2 returns "2"...
            strConverterMock.Setup(c => c.Encode(It.IsAny<byte[]>(), It.IsAny<EnvCryptAlgoEnum>()))
                .Returns<byte[]>(b => b.Length.ToString());

            // Act
            var mapper = new DatToXmlMapper(strConverterMock.Object);
            var res = mapper.Map(datPoco);

            // Assert
            res.Items.Should().NotBeNull();
            res.Items.Should().HaveCount(2, "Production & UAT categories containing sets of entries");
            res.Items[0].Name.Should().Be("Production");
            res.Items[1].Name.Should().Be("UAT");

            res.Items[0].Entry.Should().NotBeNull();
            res.Items[0].Entry.Should().HaveCount(2);
            res.Items[1].Entry.Should().NotBeNull();
            res.Items[1].Entry.Should().HaveCount(1);

            res.Items[1].Entry[0].Name.Should().Be("root password");
            res.Items[1].Entry[0].EncryptedValue.Should().HaveCount(1);
            res.Items[1].Entry[0].EncryptedValue[0].Value.Should().Be("4");

            res.Items[1].Entry[0].Decryption.Should().NotBeNull();
            res.Items[1].Entry[0].Decryption.KeyName.Should().Be("uat key");
            res.Items[1].Entry[0].Decryption.Algo.Should().Be(EnvCryptAlgoEnum.Aes.ToString());
            res.Items[1].Entry[0].Decryption.Hash.Should().Be(2);
        }



        [Test]
        public void Given_POCOWithPlainText_When_Mapped_Then_EncryptionInXMLNotSet()
        {
            // Arrange
            var datPoco = new EnvCryptDat()
            {
                Categories = new[]
                {
                    new Category()
                    {
                        Name = "Production",
                        Entries = new[]
                        {
                            new Entry()
                            {
                                Name = "database URL",
                                EncryptionAlgorithm = EnvCryptAlgoEnum.PlainText,
                                EncryptedValue = new[]
                                {
                                    new byte[1],
                                }
                            },
                        }
                    }
                }
            };

            var strConverterMock = new Mock<IEncryptedDetailsPersistConverter>();
            //      Array of length 1 returns "1", length 2 returns "2"...
            strConverterMock.Setup(c => c.Encode(It.IsAny<byte[]>(), It.IsAny<EnvCryptAlgoEnum>()))
                .Returns<byte[]>(b => b.Length.ToString());

            // Act
            var mapper = new DatToXmlMapper(strConverterMock.Object);
            var res = mapper.Map(datPoco);

            // Assert
            res.Items.Should().NotBeNull();
            res.Items.Should().HaveCount(1);
            res.Items[0].Entry.Should().NotBeNull();
            res.Items[0].Entry.Should().HaveCount(1);

            res.Items[0].Entry[0].Name.Should().Be("database URL");
            res.Items[0].Entry[0].Decryption.Should().BeNull();
            res.Items[0].Entry[0].EncryptedValue.Should().HaveCount(1);
            res.Items[0].Entry[0].EncryptedValue[0].Value.Should().Be("1");
        }
    }
}