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
        public void Given_CategoryExists_When_SearchForEntry_Then_EntryFound()
        {
            // Arrange
            const string categoryName = "The Simpsons";
            const string entryName = "Bart";
            var dat = new EnvCryptDat()
            {
                Categories = new[]
                {
                    new Category()
                    {
                        Name = categoryName,
                        Entries = new []
                        {
                            new Entry()
                            {
                                Name = "bart",
                                KeyName = "bart"
                            },
                            new Entry()
                            {
                                Name = entryName,
                                KeyName = entryName
                            }
                        }
                    }
                }
            };

            // Act
            Entry foundEntry;
            var hasBeenFound = dat.SearchForEntry(categoryName, entryName, out foundEntry);

            // Assert
            hasBeenFound.Should().BeTrue("entry exists in the DAT POCO");
            foundEntry.Name.Should().Be(entryName);
            foundEntry.KeyName.Should().Be(entryName);
        }


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
                Categories = new List<Category>() { categoryToAddTo }
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


        [Test]
        public void Given_CategoryExists_When_RemoveEntry_Then_CorrectEntryRemoved()
        {
            // Arrange
            const string categoryToRemove = "Family Guy";
            const string entryToRemove = "brian";
            var dat = new EnvCryptDat()
            {
                Categories = new[]
                {
                    new Category()
                    {
                        Name = categoryToRemove,
                        Entries = new List<Entry>()
                        {
                            new Entry()
                            {
                                Name = "bart",
                                KeyName = "bart"
                            },
                            new Entry()
                            {
                                Name = entryToRemove,
                                KeyName = entryToRemove
                            }
                        }
                    },
                    new Category()
                    {
                        Name = "some other entry",
                        Entries = new List<Entry>()
                        {
                            new Entry()
                            {
                                Name = entryToRemove,
                                KeyName = entryToRemove
                            }
                        }
                    }
                }
            };

            // Act
            var hasBeenFound = dat.RemoveEntry(categoryToRemove, entryToRemove);

            // Assert
            hasBeenFound.Should().BeTrue("entry exists in the DAT POCO");
            dat.Categories.Should().HaveCount(2);
            dat.Categories[0].Entries.Should().HaveCount(1);
            dat.Categories[1].Entries.Should().HaveCount(1);
            dat.Categories[0].Name.Should().Be(categoryToRemove, "there is still an entry in the category so the category should remain");
            dat.Categories[0].Entries[0].Name.Should().NotBe(entryToRemove);
        }


        [Test]
        public void Given_EntryExistsAndIsTheOnlyEntryInCategory_When_RemoveEntry_Then_CategoryRemoved()
        {
            // Arrange
            const string categoryToRemove = "Family Guy";
            const string entryToRemove = "brian";
            var dat = new EnvCryptDat()
            {
                Categories = new List<Category>()
                {
                    new Category()
                    {
                        Name = categoryToRemove,
                        Entries = new List<Entry>()
                        {
                            new Entry()
                            {
                                Name = entryToRemove,
                                KeyName = entryToRemove
                            }
                        }
                    },
                    new Category()
                    {
                        Name = "some other entry",
                        Entries = new List<Entry>()
                        {
                            new Entry()
                            {
                                Name = entryToRemove,
                                KeyName = entryToRemove
                            }
                        }
                    }
                }
            };

            // Act
            var hasBeenFound = dat.RemoveEntry(categoryToRemove, entryToRemove);

            // Assert
            hasBeenFound.Should().BeTrue("entry exists in the DAT POCO");
            dat.Categories.Should().HaveCount(1);
            dat.Categories[0].Name.Should()
                .NotBe(categoryToRemove, "category was empty after removal of {0} entry so should be removed",
                    entryToRemove);
        }
    }
}