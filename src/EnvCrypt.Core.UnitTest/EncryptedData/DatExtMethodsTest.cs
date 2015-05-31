using System;
using System.Collections.Generic;
using EnvCrypt.Core.EncrypedData;
using EnvCrypt.Core.EncrypedData.Poco;
using EnvCrypt.Core.EncryptionAlgo;
using EnvCrypt.Core.Key.PlainText;
using FluentAssertions;
using NUnit.Framework;

namespace EnvCrypt.Core.UnitTest.EncryptedData
{
    [TestFixture]
    public class DatExtMethodsTest
    {
        [Test]
        public void Given_NewEntryInExistingCategory_When_InsertNewEntry_Then_EntryInserted()
        {
            // Arrange
            var categoryToAddTo = new Category()
            {
                Name = "ProD",
                Entries = new List<Entry>()     // Start off with no entries
            };
            var dat = new EnvCryptDat()
            {
                Categories = new[] {categoryToAddTo}
            };

            const string newEntryName = "password";
            var newKey = new PlainTextKey();
            var encryptedData = new List<byte[]>();

            // Act
            dat.AddEntry(categoryToAddTo.Name, newEntryName, newKey, encryptedData);

            // Assert
            dat.Categories.Should().HaveCount(1);
            dat.Categories[0].Should().NotBeNull();
            dat.Categories[0].Entries.Should().HaveCount(1);
            dat.Categories[0].Entries[0].Name.Should().Be(newEntryName);
            dat.Categories[0].Entries[0].EncryptedValue.Should().Equal(encryptedData);
            dat.Categories[0].Entries[0].EncryptionAlgorithm.Should().Be(EnvCryptAlgoEnum.PlainText);
        }


        [Test]
        public void Given_NewEntryInNonExistantCategory_When_InsertNewEntry_Then_EntryInserted()
        {
            // Arrange
            var dat = new EnvCryptDat()
            {
                Categories = new List<Category>()       // Start off with no categories (thus no entries)
            };

            const string newEntryName = "password";
            var newKey = new PlainTextKey();
            var encryptedData = new List<byte[]>();

            // Act
            dat.AddEntry("UAT", newEntryName, newKey, encryptedData);

            // Assert
            dat.Categories.Should().HaveCount(1);
            dat.Categories[0].Name.Should().Be("UAT");
            dat.Categories[0].Should().NotBeNull();
            dat.Categories[0].Entries.Should().HaveCount(1);
            dat.Categories[0].Entries[0].Name.Should().Be(newEntryName);
            dat.Categories[0].Entries[0].EncryptedValue.Should().Equal(encryptedData);
            dat.Categories[0].Entries[0].EncryptionAlgorithm.Should().Be(EnvCryptAlgoEnum.PlainText);
        }


        [Test]
        public void Given_NewEntryWhenEntryAlreadyExists_When_InsertNewEntryAndOverwriteChosen_Then_ExistingEntryReplaced()
        {
            // Arrange
            const string newEntryName = "password";
            var newKey = new PlainTextKey();
            var encryptedData = new List<byte[]>();
            var categoryToAddTo = new Category()
            {
                Name = "Dev",
                Entries = new List<Entry>()
                {
                    new Entry()
                    {
                        Name = newEntryName,
                        EncryptedValue = new List<byte[]>(),
                        EncryptionAlgorithm = EnvCryptAlgoEnum.Aes,
                        KeyHash = 123,
                        KeyName = "this key's details will be overwritten"
                    }
                }
            };
            var dat = new EnvCryptDat()
            {
                Categories = new[] { categoryToAddTo }
            };

            // Act
            dat.AddEntry(categoryToAddTo.Name, newEntryName, newKey, encryptedData, true);

            // Assert
            dat.Categories.Should().HaveCount(1);
            dat.Categories[0].Should().NotBeNull();
            dat.Categories[0].Entries.Should().HaveCount(1);
            dat.Categories[0].Entries[0].Name.Should().Be(newEntryName);
            dat.Categories[0].Entries[0].EncryptedValue.Should().Equal(encryptedData);
            dat.Categories[0].Entries[0].EncryptionAlgorithm.Should().Be(EnvCryptAlgoEnum.PlainText);
        }


        [Test]
        public void Given_NewEntryWhenEntryAlreadyExists_When_InsertNewEntryAndOverwriteNotChosen_Then_Exception()
        {
            // Arrange
            const string newEntryName = "password";
            var newKey = new PlainTextKey();
            var encryptedData = new List<byte[]>();
            var categoryToAddTo = new Category()
            {
                Name = "Dev",
                Entries = new List<Entry>()
                {
                    new Entry()
                    {
                        Name = newEntryName,
                    }
                }
            };
            var dat = new EnvCryptDat()
            {
                Categories = new[] { categoryToAddTo }
            };

            // Act
            Action act = () => dat.AddEntry(categoryToAddTo.Name, newEntryName, newKey, encryptedData, false);

            // Assert
            act.ShouldThrow<EnvCryptException>();
        }
    }
}